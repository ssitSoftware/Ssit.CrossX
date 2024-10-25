using System.Numerics;
using Foundation;
using Metal;
using MetalKit;
using Ssit.Pixel.Core;
using Ssit.Pixel.Graphics;

namespace Ssit.Pixel.NET.Graphics;

internal class RenderingDeviceImpl : NSObject, IRenderingDevice
{
    private readonly MTKView _mtkView;
    
    private IMTLCommandQueue _commandQueue;
    public IRenderer Renderer { get; }
    public IRenderTarget RenderTarget { get; private set; }

    private IMTLCommandBuffer _currentCommandBuffer;
    
    public Size TargetSize
    {
        get
        {
            return new Size((int) _mtkView.Layer.Bounds.Width, (int) _mtkView.Layer.Bounds.Height);
        }
    }
    
    public MTKView View => _mtkView;

    public RenderingDeviceImpl(MTKView mtkView)
    {
        _mtkView = mtkView;
        _commandQueue = mtkView!.Device!.CreateCommandQueue();
        
        Renderer = new RendererImpl(this);
    }
    
    public void SetRenderTarget(IRenderTarget renderTarget)
    {
        //RenderTarget = renderTarget;
    }

    public IMTLCommandBuffer CommandBuffer() => _currentCommandBuffer;

    public void Draw(MTKView view, IApp app)
    {
        _currentCommandBuffer = _commandQueue.CommandBuffer()!;
        var drawable = view.CurrentDrawable;

        app.Draw();
        
        _currentCommandBuffer.PresentDrawable(drawable!);
        _currentCommandBuffer.Commit();
    }
}