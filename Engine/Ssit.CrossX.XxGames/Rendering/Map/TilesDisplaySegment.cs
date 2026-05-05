using System;
using System.Collections.Generic;
using Ssit.CrossX.Content;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;

namespace Ssit.CrossX.XxGames.Rendering.Map;

public class TilesDisplaySegment(IContentManager contentManager, TilesDisplaySegment.Parameters parameters)
    : IDisposable
{
    public class Parameters
    {
        public IReadOnlyList<Quad> Quads;
        public string TexturePath;
    }
    
    public ResourceHandle<ITexture> Texture { get; } = contentManager.Get<ITexture>(parameters.TexturePath);
    public IReadOnlyList<Quad> Quads { get; } = parameters.Quads;

    public void Dispose()
    {
        Texture?.Dispose();
    }
}