using System;
using Ssit.CrossX.Content;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.IoC;

namespace Ssit.CrossX.Games.Rendering.Map;

public class TilesDisplaySegment: IDisposable
{
    public class Parameters
    {
        public VertexPositionColorTexture[] Vertices;
        public string TexturePath;
    }
    
    public ResourceHandle<ITexture> Texture { get; }
    public IVertexBuffer VertexBuffer { get; }
    public RectangleF BoundingBox { get; }

    public TilesDisplaySegment(IIoCContainer container, IContentManager contentManager, Parameters parameters)
    {
        Texture =  contentManager.Get<ITexture>(parameters.TexturePath);
        VertexBuffer = container.IoCConstruct<IVertexBuffer>(new CreatePctVertexBufferParameters
        {
            Vertices = parameters.Vertices
        });

        BoundingBox = VertexBuffer.Bounds;
    }

    public void Dispose()
    {
        Texture?.Dispose();
        VertexBuffer?.Dispose();
    }
}