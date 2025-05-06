using System.Collections.Generic;

namespace Ssit.CrossX.Graphics.Renderer;

public interface IIndexedQuadsRenderer
{
    void Draw(ITexture texture, RectangleF target, Rectangle? source = null);
    void Draw(ITexture texture, IReadOnlyList<Quad> quads);
}