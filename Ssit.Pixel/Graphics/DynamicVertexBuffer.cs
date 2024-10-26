using System;

namespace Ssit.Pixel.Graphics;

public class DynamicVertexBuffer<TVertex> where TVertex: struct
{
    private readonly TVertex[] _vertices;
    private readonly int _structSize;
    public int Offset { get; private set; }
    public int Size { get; }

    public int Stride => _structSize;
    
    public DynamicVertexBuffer(int bufferSize, int structSize)
    {
        _vertices = new TVertex[bufferSize];
        _structSize = structSize;
        Size = bufferSize;
    }
    
    public void AddVertex(TVertex vertex)
    {
        _vertices[Offset++] = vertex;
    }

    public void Reset()
    {
        Offset = 0;
    }

    public void CopyDataTo(IntPtr dataPtr, int size)
    {
        unsafe
        {
            fixed (void* vertices = _vertices)
            {
                void* target = (void*) dataPtr;
                Buffer.MemoryCopy(vertices, target, size, Offset * _structSize);
            }
        }
    }
}