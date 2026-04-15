using System;
using System.Collections.Generic;
using System.Numerics;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.Utils;
using Ssit.IoC;

namespace Ssit.CrossX.Core;

public class PixelAppHost: IAppHost
{
    public enum Mode
    {
        Width,
        Height,
        WidthAndHeight,
        WidthAndHeightKeepAspect,
    }
    
    public class Parameters
    {
        public Size DesignSize = new(360, 180);
        public Mode Mode = Mode.WidthAndHeight;
        public int MaxScale = 2;
        public int MinScale = 1;
        public GlowParameters GlowParameters;
        public CrtParameters CrtParameters;
        public bool PixelPerfect = false;
    }
    
    public class CrtParameters
    {
        public Vector2 DisplacementFactorR;
        public Vector2 DisplacementFactorG;
        public Vector2 DisplacementFactorB;
        
        public float Interline;
        public float VerticalInterline;
        public bool HasDisplacement => DisplacementFactorR != Vector2.Zero || DisplacementFactorG != Vector2.Zero || DisplacementFactorB != Vector2.Zero;

        public float LampGlow;
        public float MotionBlur;
        public int LampDownSize = 8;
        
        public float Distortion = 0f;
        public float Vignette = 0f;
        public float VignettePower = 0f;

        public int NoiseCount = 0;
        public float NoiseIntensity = 0f;
    }

    public class GlowParameters
    {
        public float[][] Blur;
        public float BlurDivider;
        public float SelfGlowFactor;
        public bool EnableGameGlow;
        public Vector2 DisplacementFactorR;
        public Vector2 DisplacementFactorG;
        public Vector2 DisplacementFactorB;
        public bool HasDisplacement => DisplacementFactorR != Vector2.Zero || DisplacementFactorG != Vector2.Zero || DisplacementFactorB != Vector2.Zero;
    }

    private readonly Parameters _parameters;
    private readonly IIoCContainer _iocContainer;
    private readonly IRenderer2 _renderer;
    private IRenderTarget _renderTarget;
    private IRenderTarget _glowRenderTarget;
    private IRenderTarget _lampGlowRenderTarget;
    private IRenderTarget _postRenderTarget;
    private RectangleF _previousTargetRect;

    public int Scale { get; private set; } = 1;
    private int _finalScale = 1;
    
    public Size TargetSize => _renderTarget?.Size ?? new Size(800, 600);
    public Size DesignTargetSize => _renderTarget?.Size / Scale ?? new Size(800, 600);

    public Matrix3x2 Transform { get; private set; } = Matrix3x2.Identity;
    public Matrix3x2 TransformInv { get; private set; } = Matrix3x2.Identity;

    private readonly List<Vertex> _barrelVertices = new();
    private float _previousCrtDistortion = 0;

    public PixelAppHost(Parameters parameters, IIoCContainer iocContainer, IRenderer2 renderer)
    {
        _parameters = parameters;
        _iocContainer = iocContainer;
        _renderer = renderer;
    }

    public void Resize(SizeF size, bool forceRecreation = false)
    {
        (Scale, _finalScale, var targetSize) = CalculateScaleAndSize(size);
        ResizeInternal(targetSize, forceRecreation);
    }

    public void Render(object state, Action<object> renderAction)
    {
        BeginRender();
        try
        {
            _renderer.StateManager.Reset();
            _renderer.StateManager.SetGlowMode(false);
            renderAction(state);

            if (_glowRenderTarget != null)
            {
                _renderer.SetRenderTarget(_glowRenderTarget);
                if (_parameters.CrtParameters?.MotionBlur > 0)
                {
                    var factor = 1 - _parameters.CrtParameters.MotionBlur;
                    _renderer.GeometryRenderer.FillRectangle(new RectangleF(Vector2.Zero, _glowRenderTarget.Size), RgbaColor.Black * factor);
                }
                else
                {
                    _renderer.Clear(RgbaColor.Black);
                }

                if (true == _parameters.GlowParameters?.EnableGameGlow)
                {
                    _renderer.StateManager.SetGlowMode(true);
                    renderAction(state);
                    _renderer.StateManager.SetGlowMode(false);
                }
            }
        }
        finally
        {
            EndRender();
        }
    }

    private void BeginRender()
    {
        if (_renderTarget is null)
        {
            Resize(_renderer.TargetSize);
        }

        if (_parameters.CrtParameters?.MotionBlur > 0)
        {
            _renderer.SetRenderTarget(_glowRenderTarget);
            _renderer.SpriteRenderer.Draw(_renderTarget, Vector2.Zero);
        }
        
        _renderer.SetRenderTarget(_renderTarget);
    }
    
    private Vector2[] _noise = new Vector2[1024];
    
    private void EndRender()
    {
        _renderer.StateManager.SetTextureFilter(TextureFilter.Nearest);

        var sourceTexture = _renderTarget;

        if (_parameters.CrtParameters?.Interline > 0)
        {
            _renderer.SetRenderTarget(_renderTarget);
            _renderer.StateManager.SetBlendMode(BlendMode.Multiply);

            var height = _renderTarget.Size.Height / Scale;

            float val = 1 - (_parameters.CrtParameters?.Interline ?? 0);
            var color = new RgbaColor(val, val, val);
            
            for (var idx = 0; idx < height; ++idx)
            {
                _renderer.GeometryRenderer.FillRectangle(
                    new RectangleF(0, idx * Scale, _renderTarget.Size.Width, Scale / 2f), color);
            }
        }
        
        if(_parameters.CrtParameters?.VerticalInterline > 0)
        {
            var width = _renderTarget.Size.Width / Scale;

            for (var idx = 0; idx < width; ++idx)
            {
                _renderer.GeometryRenderer.FillRectangle(
                    new RectangleF(idx * Scale, 0, Scale / 2f, _renderTarget.Size.Height), RgbaColor.Black * (_parameters.CrtParameters?.VerticalInterline ?? 0));
            }
        }

        if (_parameters.CrtParameters?.NoiseCount > 0)
        {
            _renderer.SetRenderTarget(_renderTarget);
            _renderer.StateManager.SetBlendMode(BlendMode.AlphaBlend);
            
            var height = _renderTarget.Size.Height;
            var width = _renderTarget.Size.Width;

            var count = _parameters.CrtParameters.NoiseCount * Scale * Scale;

            if (_noise.Length != count)
            {
                _noise = new Vector2[count];
            }
            
            for (var idx = 0; idx < count; ++idx)
            {
                var x = (float)MersenneTwister.Shared.Next(0, (double)width);
                var y = (float)MersenneTwister.Shared.Next(0, (double)height);

                _noise[idx] = new Vector2(x, y);
            }
            
            _renderer.GeometryRenderer.DrawPoints(_noise, RgbaColor.White * _parameters.CrtParameters.NoiseIntensity);
        }
        
        if (_parameters.CrtParameters?.Interline > 0)
        {
            _renderer.SetRenderTarget(_renderTarget);
            _renderer.StateManager.SetBlendMode(BlendMode.Multiply);

            var height = _renderTarget.Size.Height / Scale;

            float val = 1 - (_parameters.CrtParameters?.Interline ?? 0);
            var color = new RgbaColor(val, val, val);
            
            for (var idx = 0; idx < height; ++idx)
            {
                _renderer.GeometryRenderer.FillRectangle(
                    new RectangleF(0, idx * Scale, _renderTarget.Size.Width, Scale / 2f), color);
            }
        }
        
        if (_glowRenderTarget != null)
        {
            _renderer.StateManager.SetBlendMode(BlendMode.Additive);

            if (_parameters.GlowParameters.SelfGlowFactor > 0.01f)
            {
                _renderer.SetRenderTarget(_glowRenderTarget);

                if (true == _parameters.GlowParameters?.HasDisplacement)
                {
                    _renderer.QuadsRenderer.Draw(_renderTarget,
                        new RectangleF(Scale * _parameters.GlowParameters.DisplacementFactorG.X,
                            Scale * _parameters.GlowParameters.DisplacementFactorG.Y,
                            _glowRenderTarget.Size.Width, _glowRenderTarget.Size.Height),
                        color: new RgbaColor(0, 255, 0) * _parameters.GlowParameters.SelfGlowFactor);

                    _renderer.QuadsRenderer.Draw(_renderTarget,
                        new RectangleF(Scale * _parameters.GlowParameters.DisplacementFactorR.X,
                            Scale * _parameters.GlowParameters.DisplacementFactorR.Y,
                            _glowRenderTarget.Size.Width, _glowRenderTarget.Size.Height),
                        color: RgbaColor.Red * _parameters.GlowParameters.SelfGlowFactor);

                    _renderer.QuadsRenderer.Draw(_renderTarget,
                        new RectangleF(Scale * _parameters.GlowParameters.DisplacementFactorB.X,
                            Scale * _parameters.GlowParameters.DisplacementFactorB.Y,
                            _glowRenderTarget.Size.Width, _glowRenderTarget.Size.Height),
                        color: RgbaColor.Blue * _parameters.GlowParameters.SelfGlowFactor);

                }
                else
                {
                    _renderer.QuadsRenderer.Draw(_renderTarget,
                        new RectangleF(0, 0,
                            _glowRenderTarget.Size.Width, _glowRenderTarget.Size.Height),
                        color: RgbaColor.White * _parameters.GlowParameters?.SelfGlowFactor);
                }
            }
            
            _renderer.SetRenderTarget(_renderTarget);

            var blur = _parameters.GlowParameters?.Blur ?? [];
            var blurDivider = _parameters.GlowParameters?.BlurDivider ?? 1;
            
            var offset = -new Vector2(Scale, Scale) * MathF.Floor(blur.Length / 2f);
            
            for (var y = 0; y < blur.Length; ++y)
            {
                for(var x = 0; x < blur[y].Length; ++x)
                {
                    var factor = blur[y][x] / blurDivider;
                    if (factor > 0.01f)
                    {
                        _renderer.QuadsRenderer.Draw(_glowRenderTarget,
                            new RectangleF(offset.X + x * Scale, offset.Y + y * Scale, _glowRenderTarget.Size.Width,
                                _glowRenderTarget.Size.Height),
                            color: RgbaColor.White * factor);
                    }
                }
            }
        }
        
        if (_postRenderTarget != null)
        {
            _renderer.SetRenderTarget(_postRenderTarget);
            _renderer.StateManager.SetTextureFilter(TextureFilter.Nearest);
            
            if (true == _parameters.CrtParameters?.HasDisplacement)
            {
                var sc = (float)_postRenderTarget.Size.Width / DesignTargetSize.Width;
                
                var rOff = _parameters.CrtParameters.DisplacementFactorR * sc;
                var gOff = _parameters.CrtParameters.DisplacementFactorG * sc;
                var bOff = _parameters.CrtParameters.DisplacementFactorB * sc;
                
                _renderer.Clear(RgbaColor.Black);
                _renderer.StateManager.SetBlendMode(BlendMode.Additive);
                
                _renderer.QuadsRenderer.Draw(sourceTexture,
                    new RectangleF(0, 0, _postRenderTarget.Size.Width, _postRenderTarget.Size.Height) + rOff, color: 0xff0000);
                
                _renderer.QuadsRenderer.Draw(sourceTexture,
                    new RectangleF(0, 0, _postRenderTarget.Size.Width, _postRenderTarget.Size.Height) + gOff, color: 0x00ff00);
                
                _renderer.QuadsRenderer.Draw(sourceTexture,
                    new RectangleF(0, 0, _postRenderTarget.Size.Width, _postRenderTarget.Size.Height) + bOff, color: 0x0000ff);
            }
            else
            {
                _renderer.StateManager.SetBlendMode(BlendMode.AlphaBlend);
                _renderer.QuadsRenderer.Draw(sourceTexture,
                    new RectangleF(0, 0, _postRenderTarget.Size.Width, _postRenderTarget.Size.Height));
            }
            
            sourceTexture = _postRenderTarget;
        }
        
        if (_parameters.CrtParameters?.LampGlow > 0 && _lampGlowRenderTarget != null)
        {
            _renderer.StateManager.SetBlendMode(BlendMode.AlphaBlend);
            _renderer.SetRenderTarget(_lampGlowRenderTarget);
            _renderer.StateManager.SetTextureFilter(TextureFilter.Linear);
            _renderer.QuadsRenderer.Draw(sourceTexture,
                new RectangleF(0, 0, _lampGlowRenderTarget.Size.Width, _lampGlowRenderTarget.Size.Height));
                
            _renderer.StateManager.SetTextureFilter(TextureFilter.Nearest);
        }
        
        if (_parameters.CrtParameters?.LampGlow > 0 && _lampGlowRenderTarget is not null)
        {
            _renderer.SetRenderTarget(sourceTexture);
            _renderer.StateManager.SetBlendMode(BlendMode.Additive);
            _renderer.StateManager.SetTextureFilter(TextureFilter.Linear);
            
            _renderer.QuadsRenderer.Draw(_lampGlowRenderTarget, new RectangleF(0, 0, sourceTexture.Size.Width, sourceTexture.Size.Height),
                color: RgbaColor.White * _parameters.CrtParameters.LampGlow);
        }

        if (_parameters.CrtParameters?.Distortion != 0)
        {
            _renderer.SetRenderTarget(sourceTexture);
            _renderer.GeometryRenderer.DrawRectangle(new RectangleF(0, 0, sourceTexture.Size.Width, sourceTexture.Size.Height), RgbaColor.Black);
        }
        
        _renderer.StateManager.SetBlendMode(BlendMode.AlphaBlend);
        _renderer.SetRenderTarget(null);
        
        var scale = MathF.Min((float)_renderer.TargetSize.Width / sourceTexture.Size.Width,
            (float)_renderer.TargetSize.Height / sourceTexture.Size.Height);

        if (_parameters.PixelPerfect)
        {
            scale = 1;
        }

        var targetSize = sourceTexture.Size.ToVector() * scale;
        var targetRect = new RectangleF((_renderer.Bounds.Size.ToVector() - targetSize) / 2f, targetSize) +
                         new Vector2(_renderer.Bounds.X, _renderer.Bounds.Y);
        
        scale = MathF.Min((float)_renderer.TargetSize.Width / _renderTarget.Size.Width,
            (float)_renderer.TargetSize.Height / _renderTarget.Size.Height);

        _renderer.StateManager.SetTextureFilter(TextureFilter.Linear);
        
        DrawToTarget(sourceTexture, targetRect);
        Transform = Matrix3x2.CreateScale(scale) * Matrix3x2.CreateTranslation(targetRect.TopLeft);
        if (Matrix3x2.Invert(Transform, out var result))
        {
            TransformInv = result;
        }
        else
        {
            TransformInv = Matrix3x2.Identity;
        }
           
        _renderer.StateManager.SetTextureFilter(TextureFilter.Nearest);
    }

    private void DrawToTarget(IRenderTarget sourceTexture, RectangleF targetRect)
    {
        var distortion = _parameters.CrtParameters?.Distortion ?? 0;
        if (MathF.Abs(distortion) < 0.01f)
        {
            _renderer.QuadsRenderer.Draw(sourceTexture, targetRect);
            return;
        }

        if (Math.Abs(_previousTargetRect.X - targetRect.X) > float.Epsilon || 
            Math.Abs(_previousTargetRect.Y - targetRect.Y) > float.Epsilon ||
            Math.Abs(_previousTargetRect.Width - targetRect.Width) > float.Epsilon || 
            Math.Abs(_previousTargetRect.Height - targetRect.Height) > float.Epsilon ||
            Math.Abs(_previousCrtDistortion - distortion) > float.Epsilon)
        {
            PrepareVertices(targetRect);
            _previousTargetRect = targetRect;
            _previousCrtDistortion = distortion;
        }
        _renderer.GeometryRenderer.DrawVertices(sourceTexture, _barrelVertices);
    }

    private void PrepareVertices(RectangleF targetRect)
    {
        var distortion = _parameters.CrtParameters?.Distortion ?? 0;
        var vignette = _parameters.CrtParameters?.Vignette ?? 0;
        var vignetteSize = _parameters.CrtParameters?.VignettePower ?? 0;
        
        var stepsX = 32;
        var stepsY = 32;
        
        var center = targetRect.Center;
        var dx = targetRect.Width / stepsX;
        var dy = targetRect.Height / stepsY;
        
        _barrelVertices.Clear();

        var size = new Vector2(targetRect.Width / 2, targetRect.Height / 2);
        
        for (var x = 0; x < stepsX; ++x)
        {
            for (var y = 0; y < stepsY; ++y)
            {
                var pos00 = targetRect.TopLeft + new Vector2(x * dx, y * dy);
                var pos01 = targetRect.TopLeft + new Vector2(x * dx, (y+1) * dy);
                var pos10 = targetRect.TopLeft + new Vector2((x+1) * dx, y * dy);
                var pos11 = targetRect.TopLeft + new Vector2((x+1) * dx, (y+1) * dy);
                
                var c00 = CalculateDistortionColor(pos00, center, size, distortion, vignette, vignetteSize);
                var c01 = CalculateDistortionColor(pos01, center, size, distortion, vignette, vignetteSize);
                var c10 = CalculateDistortionColor(pos10, center, size, distortion, vignette, vignetteSize);
                var c11 = CalculateDistortionColor(pos11, center, size, distortion, vignette, vignetteSize);
                
                pos00  = CalculateDistortion(pos00, center, size, distortion);
                pos01  = CalculateDistortion(pos01, center, size, distortion);
                pos10  = CalculateDistortion(pos10, center, size, distortion);
                pos11  = CalculateDistortion(pos11, center, size, distortion);

                var t00 = new Vector2((float)x / stepsX, (float)y / stepsY);
                var t01 = new Vector2((float)x / stepsX, (float)(y + 1) / stepsY);
                var t10 = new Vector2((float)(x + 1) / stepsX, (float)y / stepsY);
                var t11 = new Vector2((float)(x + 1) / stepsX, (float)(y + 1) / stepsY);
                
                AddQuad(_barrelVertices, pos00, pos01, pos10, pos11, t00, t01, t10, t11, c00, c01, c10, c11);
            }
        }
    }

    private void AddQuad(List<Vertex> vertices, Vector2 pos00, Vector2 pos01, Vector2 pos10, Vector2 pos11, Vector2 t00, Vector2 t01, Vector2 t10, Vector2 t11, RgbaColor c00, RgbaColor c01, RgbaColor c10, RgbaColor c11)
    {
        vertices.Add(new Vertex
        {
            Position = pos00,
            Color = c00,
            TexCoord = t00
        });
        
        vertices.Add(new Vertex
        {
            Position = pos01,
            Color = c01,
            TexCoord = t01
        });
        
        vertices.Add(new Vertex
        {
            Position = pos10,
            Color = c10,
            TexCoord = t10
        });
        
        vertices.Add(new Vertex
        {
            Position = pos10,
            Color = c10,
            TexCoord = t10
        });
        
        vertices.Add(new Vertex
        {
            Position = pos01,
            Color = c01,
            TexCoord = t01
        });
        
        vertices.Add(new Vertex
        {
            Position = pos11,
            Color = c11,
            TexCoord = t11
        });
    }
    
    private RgbaColor CalculateDistortionColor(Vector2 pos, Vector2 center, Vector2 size, float factor, float vignette, float vignettePower)
    {
        var normPos = (pos - center) / size;
        float radius = normPos.Length() / 1.42f;
        float theta = MathF.Atan2(normPos.Y, normPos.X);
        
        radius = 1 - radius;
        
        radius = MathF.Pow(radius, factor);
        
        radius = 1 - radius;
        
        var f2 = factor > 1 ? factor : 2 - factor;
        var f = MathF.Pow(1.42f, 1 / (f2 * f2));
        radius *= f;
        
        pos.X = radius * MathF.Cos(theta);
        pos.Y = radius * MathF.Sin(theta);
        
        pos = pos * size + center;

        var dist1 = (normPos - center).Length();
        var dist2 = (pos - center).Length();

        var distortion =MathF.Max(0, 1 -  MathF.Pow(dist2 / dist1, vignettePower));
        
        return RgbaColor.White.Mix(new RgbaColor(distortion, distortion, distortion, 1), vignette);
    }

    private Vector2 CalculateDistortion(Vector2 pos, Vector2 center, Vector2 size, float factor)
    {
        var normPos = (pos - center) / size;
        float radius = normPos.Length() / 1.42f;
        float theta = MathF.Atan2(normPos.Y, normPos.X);
        
        radius = 1 - radius;
        
        radius = MathF.Pow(radius, factor);
        
        radius = 1 - radius;

        var f2 = factor > 1 ? factor : 2 - factor;
        var f = MathF.Pow(1.42f, 1 / (f2 * f2));
        radius *= f;
        
        pos.X = radius * MathF.Cos(theta);
        pos.Y = radius * MathF.Sin(theta);
        
        pos = pos * size + center;

        return pos;
    }

    private (int, int, Size) CalculateScaleAndSize(SizeF size)
    {
        float scale = 1;

        switch (_parameters.Mode)
        {
            case Mode.Width:
                scale = (float)size.Width / _parameters.DesignSize.Width;
                break;
            
            case Mode.Height:
                scale = (float)size.Height / _parameters.DesignSize.Height;
                break;
            
            case Mode.WidthAndHeight:
                scale = Math.Min((float)size.Width / _parameters.DesignSize.Width,
                    (float)size.Height / _parameters.DesignSize.Height);
                break;
            
            case Mode.WidthAndHeightKeepAspect:
                scale = Math.Min((float)size.Width / _parameters.DesignSize.Width,
                    (float)size.Height / _parameters.DesignSize.Height);
                break;
        }
        
        var scaleInt = _parameters.PixelPerfect ? (int)Math.Floor(scale) : (int)Math.Ceiling(scale);
        var targetScale = Math.Max(_parameters.MinScale, Math.Min(_parameters.MaxScale, scaleInt));
        
        switch (_parameters.Mode)
        {
            case Mode.WidthAndHeightKeepAspect:
                return (targetScale, scaleInt, _parameters.DesignSize * targetScale);

            case Mode.WidthAndHeight:
            case Mode.Height:
            {
                var aspect = (float)size.Width / size.Height;
                var height = _parameters.DesignSize.Height * targetScale;
                var width = (int) MathF.Ceiling(height * aspect);
                return (targetScale, scaleInt, new Size(width, height));
            }
            case Mode.Width:
            {
                var aspect = (float)size.Width / size.Height;
                var width = _parameters.DesignSize.Width * targetScale;
                var height = (int) MathF.Ceiling(width / aspect);
                return (targetScale, scaleInt, new Size(width, height));
            }
        }
        
        throw new NotImplementedException();
    }

    private void ResizeInternal(Size size, bool forceRecreation)
    {
        var targetScale = _parameters.PixelPerfect ? (int)Math.Floor((float)_finalScale / Scale) : (int)Math.Ceiling((float)_finalScale / Scale);
        
        var postRenderSize = _postRenderTarget?.Size ?? Size.Zero;
        
        if (_renderTarget?.Size == size && postRenderSize == size * targetScale && !forceRecreation)
        {
            return;
        }
        
        _renderTarget?.Dispose();
        _postRenderTarget?.Dispose();
        _postRenderTarget = null;
        
        _glowRenderTarget?.Dispose();
        _glowRenderTarget = null;

        _lampGlowRenderTarget?.Dispose();
        _lampGlowRenderTarget = null;

        _renderTarget = _iocContainer.IoCConstruct<IRenderTarget>(new CreateRenderTargetParameters
        {
            Size = size
        });

        if (_parameters.GlowParameters is not null)
        {
            _glowRenderTarget = _iocContainer.IoCConstruct<IRenderTarget>(new CreateRenderTargetParameters
            {
                Size = size
            });
        }

        if (_parameters.CrtParameters?.LampGlow > 0)
        {
            var glowSize = new Size(Math.Max(1, size.Width / _parameters.CrtParameters.LampDownSize), Math.Max(1, size.Height / _parameters.CrtParameters.LampDownSize));
            _lampGlowRenderTarget = _iocContainer.IoCConstruct<IRenderTarget>(new CreateRenderTargetParameters
            {
                Size = glowSize
            });
        }

        //if (targetScale > 1)
        {
            _postRenderTarget = _iocContainer.IoCConstruct<IRenderTarget>(new CreateRenderTargetParameters
            {
                Size = size * targetScale
            });
        }
    }
    
    public void Dispose()
    {
        _renderTarget?.Dispose();
        _renderTarget = null;
        
        _glowRenderTarget?.Dispose();
        _glowRenderTarget = null;
        
        _postRenderTarget?.Dispose();
        _postRenderTarget = null;
        
        _lampGlowRenderTarget?.Dispose();
        _lampGlowRenderTarget = null;
    }
}