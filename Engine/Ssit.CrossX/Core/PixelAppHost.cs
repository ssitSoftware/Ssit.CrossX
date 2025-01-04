using System;
using System.Numerics;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.IoC;

namespace Ssit.CrossX.Core;

public class PixelAppHost: IDisposable
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
        public Size DesignSize = new Size(360, 180);
        public Mode Mode = Mode.WidthAndHeight;
        public IRenderer Renderer;
        public int MaxScale = 2;
    }

    private readonly Parameters _parameters;
    private readonly IIoCContainer _iocContainer;
    private IRenderTarget _renderTarget;
    private IRenderTarget _postRenderTarget;

    public int Scale { get; private set; } = 1;
    private int _finalScale = 1;
    
    public Size TargetSize => _renderTarget?.Size ?? new Size(800, 600);
    public Size DesignTargetSize => _renderTarget?.Size / Scale ?? new Size(800, 600);

    public Matrix3x2 Transform { get; private set; } = Matrix3x2.Identity;
    
    public PixelAppHost(Parameters parameters, IIoCContainer iocContainer)
    {
        _parameters = parameters;
        _iocContainer = iocContainer;
    }

    public void Resize(Size size)
    {
        (Scale, _finalScale, var targetSize) = CalculateScaleAndSize(size);
        ResizeInternal(targetSize);
    }

    public void Render(Action renderAction)
    {
        BeginRender();
        try
        {
            
            renderAction();
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
            Resize(_parameters.Renderer.TargetSize);
        }
        
        _parameters.Renderer.SetRenderTarget(_renderTarget);
    }
    
    private void EndRender()
    {
        _parameters.Renderer.SetRenderTarget(_postRenderTarget);
        _parameters.Renderer.DrawTexture(_renderTarget, new RectangleF(0, 0, _postRenderTarget.Size.Width, _postRenderTarget.Size.Height), filter: TextureFilter.Nearest);
        
        _parameters.Renderer.SetRenderTarget(null);
        
        var scale = MathF.Min((float)_parameters.Renderer.TargetSize.Width / _postRenderTarget.Size.Width,
            (float)_parameters.Renderer.TargetSize.Height / _postRenderTarget.Size.Height);

        var targetSize = _postRenderTarget.Size.ToVector() * scale;
        var targetRect = new RectangleF((_parameters.Renderer.TargetSize.ToVector() - targetSize) / 2f, targetSize);
        
        _parameters.Renderer.DrawTexture(_renderTarget, targetRect, null, filter: TextureFilter.Linear);
        
        Transform = Matrix3x2.CreateScale(scale * _finalScale) * Matrix3x2.CreateTranslation(targetRect.TopLeft);
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
        if (_renderTarget?.Size == size && _postRenderTarget?.Size == size * _finalScale / Scale)
        {
            return;
        }
        
        _renderTarget?.Dispose();
        _postRenderTarget?.Dispose();

        _renderTarget = _iocContainer.IoCConstruct<IRenderTarget>(new CreateRenderTargetParameters
        {
            Size = size
        });
        
        _postRenderTarget = _iocContainer.IoCConstruct<IRenderTarget>(new CreateRenderTargetParameters
        {
            Size = size * _finalScale
        });
    }
    
    public void Dispose()
    {
        _renderTarget?.Dispose();
        _renderTarget = null;
        
        _postRenderTarget?.Dispose();
        _postRenderTarget = null;
    }
}