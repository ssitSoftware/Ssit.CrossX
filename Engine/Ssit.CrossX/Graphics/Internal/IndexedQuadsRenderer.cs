using System.Collections.Generic;
using Ssit.CrossX.Graphics.Renderer;

namespace Ssit.CrossX.Graphics.Internal;

public class IndexedQuadsRenderer(IRenderer2 renderer) : IIndexedQuadsRenderer
{
    public void Draw(ITexture texture, RectangleF target, Rectangle? source = null) => renderer.QuadsRenderer.Draw(texture, target, source);
    public void Draw(ITexture texture, IReadOnlyList<Quad> quads) => renderer.QuadsRenderer.Draw(texture, quads, RgbaColor.White);
}