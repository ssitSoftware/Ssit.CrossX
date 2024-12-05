using System;

namespace Ssit.CrossX.Graphics.Internal;

public interface IGlyphFont: IFont, IDisposable
{
    GlyphFont.FontMetrics Metrics { get; }
    Glyph GetGlyph(char c);
    ITexture FontSheet { get; }
    ITexture OutlineSheet { get; }
}