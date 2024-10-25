namespace Ssit.Pixel.Framework.Graphics;

public interface IRenderingDevice
{
    
    IRenderer Renderer { get; }
    IRenderTarget RenderTarget { get; }
    
    void SetRenderTarget(IRenderTarget renderTarget);
}