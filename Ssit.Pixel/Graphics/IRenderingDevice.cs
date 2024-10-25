namespace Ssit.Pixel.Graphics;

public interface IRenderingDevice
{
    
    IRenderer Renderer { get; }
    IRenderTarget RenderTarget { get; }
    
    void SetRenderTarget(IRenderTarget renderTarget);
}