using System.Numerics;
using SDL;
using Ssit.CrossX.Graphics.Renderer;

namespace Ssit.CrossX.SDL.Graphics;

public class SdlVertexBuffer: IVertexBuffer
{
    public SDL_Vertex[] GetVertices(IRenderStateProvider stateProvider)
    {
        var scale = stateProvider.Scale;
        var offset = stateProvider.Offset;
        
        if (Math.Abs(scale - 1) < float.Epsilon && offset == Vector2.Zero) return _originalVertices;

        if (_transformedVertices != null && _transformOffset == offset && Math.Abs(_transformScale - scale) < float.Epsilon)
        {
            return _transformedVertices;
        }

        if (_transformedVertices is null || _transformedVertices.Length < VertexCount)
        {
            _transformedVertices = new SDL_Vertex[VertexCount];
        }

        for (var idx = 0; idx < VertexCount; idx++)
        {
            _transformedVertices[idx] = _originalVertices[idx];
            _transformedVertices[idx].position = new SDL_FPoint
            {
                x = _originalVertices[idx].position.x * scale + offset.X,
                y = _originalVertices[idx].position.y * scale + offset.Y
            };
        }
        
        _transformOffset = offset;
        _transformScale = scale;
        
        return _transformedVertices;
    }

    private SDL_Vertex[] _originalVertices;
    private SDL_Vertex[] _transformedVertices;

    private Vector2 _transformOffset;
    private float _transformScale;
    
    public int VertexCount { get; private set; }
    
    public SdlVertexBuffer()
    {
        _originalVertices = [];
    }
    
    public SdlVertexBuffer(IReadOnlyList<Vertex> vertices)
    {
        _originalVertices = Extensions.PrepareVertices(vertices, vertices.Count);
        VertexCount = vertices.Count;
    }

    public void Update(IReadOnlyList<Vertex> vertices)
    {
        _originalVertices = Extensions.PrepareVertices(vertices, vertices.Count, null, _originalVertices);
        _transformedVertices = null;
        
        VertexCount = vertices.Count;
    }
    
    void IDisposable.Dispose()
    {
        _originalVertices = null;
        _transformedVertices = null;
    }
}