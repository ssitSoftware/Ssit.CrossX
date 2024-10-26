namespace Ssit.Pixel.Graphics;

public interface IVertexBuffer
{
    RectangleF Bounds { get; }
    PrimitiveType PrimitiveType { get; }
    VertexMode VertexMode { get; }
    TType Get<TType>();
}