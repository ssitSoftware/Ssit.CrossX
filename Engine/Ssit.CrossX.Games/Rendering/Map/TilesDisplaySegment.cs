using System;
using System.Collections.Generic;
using Ssit.CrossX.Content;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.IoC;

namespace Ssit.CrossX.Games.Rendering.Map;

public class TilesDisplaySegment: IDisposable
{
    public class Parameters
    {
        public IReadOnlyList<Quad> Quads;
        public string TexturePath;
        public RgbaColor TintColor { get; set; }
    }
    
    public ResourceHandle<ITexture> Texture { get; }
    public IReadOnlyList<Quad> Quads { get; }
    public RgbaColor TintColor { get; set; }

    public TilesDisplaySegment(IContentManager contentManager, Parameters parameters)
    {
        Texture =  contentManager.Get<ITexture>(parameters.TexturePath);
        Quads = parameters.Quads;
        TintColor = parameters.TintColor;
    }

    public void Dispose()
    {
        Texture?.Dispose();
    }
}