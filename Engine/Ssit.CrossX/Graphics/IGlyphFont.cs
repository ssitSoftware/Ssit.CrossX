using System;
using Ssit.CrossX.Graphics.Internal;

namespace Ssit.CrossX.Graphics;

public interface IGlyphFont: IFont, IDisposable
{
    GlyphFont.FontMetrics Metrics { get; }
    Glyph GetGlyph(char c);
    ITexture FontSheet { get; }
    ITexture OutlineSheet { get; }
}