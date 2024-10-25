namespace Ssit.Pixel.Graphics;

public interface IVertexBuffer
{
    RectangleF Bounds { get; }
    PrimitiveType PrimitiveType { get; }
}