using System.Collections.Generic;

namespace Ssit.CrossX.Graphics.Renderer;

public interface IQuadsRenderer
{
    void Draw(ITexture texture, RectangleF target, Rectangle source, RgbaColor color);
    void Draw(ITexture texture, IReadOnlyList<Quad> quads, RgbaColor color);
}