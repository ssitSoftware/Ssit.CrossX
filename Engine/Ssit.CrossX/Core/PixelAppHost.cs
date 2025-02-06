using System;
using System.Numerics;
using Ssit.CrossX.Graphics;
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
        public GlowParameters GlowParameters;
        public bool EnableDebug = false;
    }

    public class GlowParameters
    {
        
    }

    private readonly Parameters _parameters;
    private readonly IIoCContainer _iocContainer;
    private readonly IRenderer _renderer;
    private IRenderTarget _renderTarget;
    private IRenderTarget _glowRenderTarget;
    private IRenderTarget _postRenderTarget;

    public int Scale { get; private set; } = 1;
    private int _finalScale = 1;
    
    public Size TargetSize => _renderTarget?.Size ?? new Size(800, 600);
    public Size DesignTargetSize => _renderTarget?.Size / Scale ?? new Size(800, 600);

    public Matrix3x2 Transform { get; private set; } = Matrix3x2.Identity;
    
    public PixelAppHost(Parameters parameters, IIoCContainer iocContainer, IRenderingWindow renderingWindow)
    {
        _parameters = parameters;
        _iocContainer = iocContainer;
        _renderer = renderingWindow.Renderer;
    }

    public void Resize(Size size)
    {
        (Scale, _finalScale, var targetSize) = CalculateScaleAndSize(size);
        ResizeInternal(targetSize);
    }

    public void Render(Action<RenderMode> renderAction)
    {
        BeginRender();
        try
        {
            renderAction(RenderMode.Normal);

            if (_glowRenderTarget != null)
            {
                _renderer.SetRenderTarget(_glowRenderTarget);
                _renderer.Clear(RgbaColor.Black);
                renderAction(RenderMode.Glow);
            }
        }
        finally
        {
            EndRender(renderAction);
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
    
    private void EndRender(Action<RenderMode> renderAction)
    {
        _renderer.SetRenderTarget(_postRenderTarget);
        _renderer.DrawTexture(_renderTarget,
            new RectangleF(0, 0, _postRenderTarget.Size.Width, _postRenderTarget.Size.Height),
            filter: TextureFilter.Nearest);

        if (_glowRenderTarget != null)
        {
            _renderer.DrawTexture(_glowRenderTarget,
                new RectangleF(0, 0, _postRenderTarget.Size.Width, _postRenderTarget.Size.Height),
                filter: TextureFilter.Nearest, color: RgbaColor.White * 0.5f);
        }

        var sourceTexture = _postRenderTarget;
                  
        _renderer.SetRenderTarget(null);
        
        var scale = MathF.Min((float)_renderer.TargetSize.Width / sourceTexture.Size.Width,
            (float)_renderer.TargetSize.Height / sourceTexture.Size.Height);

        var targetSize = sourceTexture.Size.ToVector() * scale;
        var targetRect = new RectangleF((_renderer.TargetSize.ToVector() - targetSize) / 2f, targetSize);
        
        scale = MathF.Min((float)_renderer.TargetSize.Width / _renderTarget.Size.Width,
            (float)_renderer.TargetSize.Height / _renderTarget.Size.Height);
        
        _renderer.DrawTexture(_renderTarget, targetRect,filter: TextureFilter.Linear);
        Transform = Matrix3x2.CreateScale(scale) * Matrix3x2.CreateTranslation(targetRect.TopLeft);

        if (_parameters.EnableDebug)
        {
            _renderer.StateManager.Reset();
            _renderer.StateManager.ClipRectangle(targetRect);
            _renderer.StateManager.Transform(Transform);
            
            renderAction(RenderMode.Debug);
            
            _renderer.StateManager.Reset();
        }
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
        var targetScale = Math.Min(_parameters.MaxScale, scaleInt);
        
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

    private void ResizeInternal(Size size)
    {
        var targetScale = (int)Math.Ceiling((float)_finalScale / Scale);
        
        var postRenderSize = _postRenderTarget?.Size ?? Size.Zero;
        
        if (_renderTarget?.Size == size && postRenderSize == size * targetScale)
        {
            return;
        }
        
        _renderTarget?.Dispose();
        _postRenderTarget?.Dispose();
        _glowRenderTarget?.Dispose();
        _glowRenderTarget = null;

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
        
        _postRenderTarget = _iocContainer.IoCConstruct<IRenderTarget>(new CreateRenderTargetParameters
        {
            Size = size * targetScale
        });
    }
    
    public void Dispose()
    {
        _renderTarget?.Dispose();
        _renderTarget = null;
        
        _glowRenderTarget?.Dispose();
        _glowRenderTarget = null;
        
        _postRenderTarget?.Dispose();
        _postRenderTarget = null;
    }
}