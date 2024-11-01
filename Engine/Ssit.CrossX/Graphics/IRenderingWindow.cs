namespace Ssit.CrossX.Graphics;

public interface IRenderingWindow
{
    IRenderer Renderer { get; }
    Size Size { get; }
}