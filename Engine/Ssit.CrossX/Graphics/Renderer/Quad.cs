namespace Ssit.CrossX.Graphics.Renderer;

public readonly struct Quad(RectangleF target, Rectangle source)
{
    public readonly RectangleF Target = target;
    public readonly Rectangle Source = source;
}