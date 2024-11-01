#if __IOS__ || __MACCATALYST__

using System;
using Foundation;
using Metal;
using MetalKit;
using Ssit.CrossX.Core;
using Ssit.CrossX.Graphics;

namespace Ssit.CrossX.NET.Apple.Graphics;

internal class RenderingWindowImpl : NSObject, IRenderingWindow, IMetalDevice
{
    private readonly MTKView _mtkView;
    
    private IMTLCommandQueue _commandQueue;
    public IRenderer Renderer { get; }
    public IRenderTarget CurrentRenderTarget { get; set; }

    private IMTLCommandBuffer _currentCommandBuffer;

    public MTKView MetalView => _mtkView;

    public Size TargetSize => CurrentRenderTarget?.Size ?? Size;

    public Size Size => new((int) _mtkView.Layer.Bounds.Width.Value, (int) _mtkView.Layer.Bounds.Height.Value);
    
    public MTKView View => _mtkView;

    public RenderingWindowImpl(MTKView mtkView)
    {
        _mtkView = mtkView;
        _commandQueue = mtkView!.Device!.CreateCommandQueue();
        
        Renderer = new RendererImpl(this);
    }
    
    public void SetRenderTarget(IRenderTarget renderTarget)
    {
        CurrentRenderTarget = renderTarget;
    }

    public IMTLCommandBuffer CommandBuffer => _currentCommandBuffer;

    public void CommitCommandBuffer()
    {
        _currentCommandBuffer.Commit();
        _currentCommandBuffer.WaitUntilCompleted();
        _currentCommandBuffer.Dispose();
        
        _currentCommandBuffer = _commandQueue.CommandBuffer()!;
    }
    
    public void Draw(MTKView view, IApp app)
    {
        _currentCommandBuffer = _commandQueue.CommandBuffer()!;
        var drawable = view.CurrentDrawable;

        Renderer.SetTransform(null);
        app.Draw();
        Renderer.Flush();
        
        _currentCommandBuffer.PresentDrawable(drawable!);
        _currentCommandBuffer.Commit();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _currentCommandBuffer?.Dispose();
            _currentCommandBuffer = null;
            _commandQueue.Dispose();

            (Renderer as IDisposable)?.Dispose();
        }
    }
}

#endif