namespace Ssit.Pixel.Framework.Graphics;

public interface IVertexBuffer
{
    RectangleF Bounds { get; }
    PrimitiveType PrimitiveType { get; }
}