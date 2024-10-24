namespace Ssit.Pixel.Framework.Graphics;

public interface IRenderingDevice
{
    Size ScreenSize { get; }

    IRenderer Renderer { get; }
    IRenderTarget RenderTarget { get; }
    
    void SetRenderTarget(IRenderTarget renderTarget);
}