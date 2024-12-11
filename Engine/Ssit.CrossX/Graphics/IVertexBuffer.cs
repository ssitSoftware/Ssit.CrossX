using System;

namespace Ssit.CrossX.Graphics;

public interface IVertexBuffer: IDisposable
{
    RectangleF Bounds { get; }
    PrimitiveType PrimitiveType { get; }
    VertexMode VertexMode { get; }
    TType Get<TType>();
    int Length { get; }
    void Update<TVertex>(TVertex[] vertices) where TVertex : unmanaged;
}