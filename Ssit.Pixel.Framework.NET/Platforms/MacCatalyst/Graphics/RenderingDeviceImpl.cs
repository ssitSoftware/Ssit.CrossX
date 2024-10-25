using Ssit.Pixel.Framework.Graphics;
using static SDL2.Bindings.SDL;

namespace Ssit.Pixel.Framework.NET.Graphics;

internal class RenderingDeviceImpl : IRenderingDevice
{
    private readonly IntPtr _sdlRendererHandle;
    
    public IRenderer Renderer { get; }
    public IRenderTarget RenderTarget { get; private set; }

    public RenderingDeviceImpl(IntPtr sdlRendererHandle)
    {
        _sdlRendererHandle = sdlRendererHandle;
        Renderer = new RendererImpl(sdlRendererHandle, this);
    }
    
    public void SetRenderTarget(IRenderTarget renderTarget)
    {
        RenderTarget = renderTarget;
    }
}