using System;
using System.Numerics;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.IoC;

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
    }
    
    public class CrtParameters
    {
        public Vector2 DisplacementFactorR;
        public Vector2 DisplacementFactorG;
        public Vector2 DisplacementFactorB;
        
        public float Interline;
        public bool HasDisplacement => DisplacementFactorR != Vector2.Zero || DisplacementFactorG != Vector2.Zero || DisplacementFactorB != Vector2.Zero;

        public float LampGlow;
        public int LampDownSize = 8;
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

    public int Scale { get; private set; } = 1;
    private int _finalScale = 1;
    
    public Size TargetSize => _renderTarget?.Size ?? new Size(800, 600);
    public Size DesignTargetSize => _renderTarget?.Size / Scale ?? new Size(800, 600);

    public Matrix3x2 Transform { get; private set; } = Matrix3x2.Identity;
    
    public PixelAppHost(Parameters parameters, IIoCContainer iocContainer, IRenderer2 renderer)
    {
        _parameters = parameters;
        _iocContainer = iocContainer;
        _renderer = renderer;
    }

    public void Resize(Size size, bool forceRecreation = false)
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
                _renderer.Clear(RgbaColor.Black);
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
        
        _renderer.SetRenderTarget(_renderTarget);
    }
    
    private void EndRender()
    {
        _renderer.StateManager.SetTextureFilter(TextureFilter.Nearest);

        var sourceTexture = _renderTarget;

        if (_parameters.CrtParameters?.Interline > 0)
        {
            _renderer.SetRenderTarget(_renderTarget);
            _renderer.StateManager.SetBlendMode(BlendMode.Multiply);
            
            var height = _renderTarget.Size.Height / Scale;

            for (var idx = 0; idx < height; ++idx)
            {
                _renderer.GeometryRenderer.FillRectangle(
                    new RectangleF(0, idx * Scale, _renderTarget.Size.Width, Scale / 2f), RgbaColor.Black * (_parameters.CrtParameters?.Interline ?? 0));
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
        
        if (_parameters.CrtParameters?.Interline > 0)
        {
            _renderer.SetRenderTarget(_renderTarget);
            _renderer.StateManager.SetBlendMode(BlendMode.Multiply);
            
            var height = _renderTarget.Size.Height / Scale;

            for (var idx = 0; idx < height; ++idx)
            {
                _renderer.GeometryRenderer.FillRectangle(
                    new RectangleF(0, idx * Scale, _renderTarget.Size.Width, Scale / 2f), RgbaColor.Black * (_parameters.CrtParameters?.Interline ?? 0));
            }
        }
        
        if (_postRenderTarget != null)
        {
            _renderer.SetRenderTarget(_postRenderTarget);

            if (true == _parameters.CrtParameters?.HasDisplacement)
            {
                var sc = (float)_postRenderTarget.Size.Width / sourceTexture.Size.Width;
                
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
        
        _renderer.StateManager.SetBlendMode(BlendMode.AlphaBlend);
        _renderer.SetRenderTarget(null);
        
        var scale = MathF.Min((float)_renderer.TargetSize.Width / sourceTexture.Size.Width,
            (float)_renderer.TargetSize.Height / sourceTexture.Size.Height);

        var targetSize = sourceTexture.Size.ToVector() * scale;
        var targetRect = new RectangleF((_renderer.TargetSize.ToVector() - targetSize) / 2f, targetSize);

        scale = MathF.Min((float)_renderer.TargetSize.Width / _renderTarget.Size.Width,
            (float)_renderer.TargetSize.Height / _renderTarget.Size.Height);

        _renderer.StateManager.SetTextureFilter(TextureFilter.Linear);
        _renderer.QuadsRenderer.Draw(sourceTexture, targetRect);
        
        Transform = Matrix3x2.CreateScale(scale) * Matrix3x2.CreateTranslation(targetRect.TopLeft);
        
        _renderer.StateManager.SetTextureFilter(TextureFilter.Nearest);
    }

    private (int, int, Size) CalculateScaleAndSize(Size size)
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
        
        var scaleInt = (int)Math.Ceiling(scale);
        var targetScale = Math.Max(_parameters.MinScale, Math.Min(_parameters.MaxScale, scaleInt));
        
        switch (_parameters.Mode)
        {
            case Mode.WidthAndHeightKeepAspect:
                return (targetScale, scaleInt, _parameters.DesignSize * targetScale);

            case Mode.Height:
            {
                var aspect = (float)size.Width / size.Height;
                var height = _parameters.DesignSize.Height * targetScale;
                var width = (int) MathF.Ceiling(height * aspect);
                return (targetScale, scaleInt, new Size(width, height));
            }
        }
        
        throw new NotImplementedException();
    }

    private void ResizeInternal(Size size, bool forceRecreation)
    {
        var targetScale = (int)Math.Ceiling((float)_finalScale / Scale);
        
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

        if (targetScale > 1)
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