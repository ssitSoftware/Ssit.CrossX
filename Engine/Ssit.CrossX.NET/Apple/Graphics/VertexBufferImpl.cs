#if __IOS__ || __MACCATALYST__

using System;
using Metal;
using Ssit.CrossX.Graphics;

namespace Ssit.CrossX.NET.Apple.Graphics;

public class VertexBufferImpl: IVertexBuffer
{
    public RectangleF Bounds { get; }
    public PrimitiveType PrimitiveType { get; }
    public VertexMode VertexMode { get; }
    
    private IMTLBuffer _mtlVertexBuffer;
    
    public TType Get<TType>()
    {
        if (typeof(TType) == typeof(IMTLBuffer))
        {
            return (TType)_mtlVertexBuffer;
        }

        throw new InvalidCastException();
    }

    public int Length { get; }

    public VertexBufferImpl(IMetalDevice metalDevice, CreatePctVertexBufferParameters parameters)
    {
        PrimitiveType = PrimitiveType.Triangles;
        VertexMode = VertexPositionColorTexture.Mode;

        float minX = float.MaxValue, minY = float.MaxValue;
        float maxX = float.MinValue, maxY = float.MinValue;

        foreach (var vertex in parameters.Vertices)
        {
            minX = MathF.Min(vertex.Position.X, minX);
            minY = MathF.Min(vertex.Position.Y, minY);
            
            maxX = MathF.Max(vertex.Position.X, maxX);
            maxY = MathF.Max(vertex.Position.Y, maxY);
        }
        
        Bounds = new RectangleF(minX, minY, maxX - minX, maxY - minY);
        
        _mtlVertexBuffer =
            metalDevice.MetalView.Device!.CreateBuffer(parameters.Vertices, MTLResourceOptions.StorageModeShared);
        
        Length = parameters.Vertices.Length;
    }

    public void Dispose()
    {
        _mtlVertexBuffer?.Dispose();
        _mtlVertexBuffer = null;
    }
}

#endif