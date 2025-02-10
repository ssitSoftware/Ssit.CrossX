namespace Ssit.CrossX.Graphics.Renderer;

public readonly struct Quad(RectangleF target, RectangleF source)
{
    public readonly RectangleF Target = target;
    public readonly RectangleF Source = source;
}