using System.Collections.Generic;

namespace Ssit.CrossX.Graphics.Renderer;

public interface IQuadsRenderer
{
    int QuadsRendered { get; }
    void Draw(ITexture texture, RectangleF target, Rectangle? source = null, RgbaColor? color = null);
    void Draw(ITexture texture, IReadOnlyList<Quad> quads, RgbaColor color);
}