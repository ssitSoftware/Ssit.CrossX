using Foundation;
using Metal;
using MetalKit;
using Ssit.Pixel.Core;
using Ssit.Pixel.Graphics;

namespace Ssit.Pixel.NET.Graphics;

internal class RenderingDeviceImpl : NSObject, IRenderingDevice, IMetalDevice
{
    private readonly MTKView _mtkView;
    
    private IMTLCommandQueue _commandQueue;
    public IRenderer Renderer { get; }
    public IRenderTarget RenderTarget { get; private set; }

    private IMTLCommandBuffer _currentCommandBuffer;

    public MTKView MetalView => _mtkView;
    
    public Size TargetSize => RenderTarget?.Size ?? new Size((int) _mtkView.Layer.Bounds.Width.Value, (int) _mtkView.Layer.Bounds.Height.Value);

    public MTKView View => _mtkView;

    public RenderingDeviceImpl(MTKView mtkView)
    {
        _mtkView = mtkView;
        _commandQueue = mtkView!.Device!.CreateCommandQueue();
        
        Renderer = new RendererImpl(this);
    }
    
    public void SetRenderTarget(IRenderTarget renderTarget)
    {
        RenderTarget = renderTarget;
    }

    public IMTLCommandBuffer CommandBuffer => _currentCommandBuffer;

    public void Draw(MTKView view, IApp app)
    {
        _currentCommandBuffer = _commandQueue.CommandBuffer()!;
        var drawable = view.CurrentDrawable;

        app.Draw();
        
        ((RendererImpl)Renderer).Flush();
        
        _currentCommandBuffer.PresentDrawable(drawable!);
        _currentCommandBuffer.Commit();
    }
}