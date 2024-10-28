namespace Ssit.Pixel.Graphics;

public interface IRenderingWindow
{
    IRenderer Renderer { get; }
    Size Size { get; }
}