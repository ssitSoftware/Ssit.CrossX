using System.Numerics;
using Ssit.CrossX.Graphics.Font;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.Text;

namespace Ssit.CrossX.Graphics.Internal;

public class IndexedTextRenderer(IRenderer2 renderer, IPaletteSource paletteSource) : IIndexedTextRenderer
{
    public void DrawText(IFont font, TextSource text, Vector2 position, byte color, ContentAlign align = ContentAlign.Left,
        TextSpacing spacing = TextSpacing.Normal, byte? outlineColor = null, TextRenderingContext context = null)
    {
        RgbaColor? outlineClr = outlineColor.HasValue ? paletteSource.Palette[outlineColor.Value] : null;
        
        renderer.TextRenderer.DrawText(font, text, position, align, 1, paletteSource.Palette[color], spacing, outlineClr, context);;
    }

    public void DrawText(IFont font, TextSource text, RectangleF position, byte color, ContentAlign align = ContentAlign.Left,
        TextSpacing spacing = TextSpacing.Normal, float paragraphSpacing = -1, byte? outlineColor = null,
        TextRenderingContext context = null)
    {
        RgbaColor? outlineClr = outlineColor.HasValue ? paletteSource.Palette[outlineColor.Value] : null;
        renderer.TextRenderer.DrawText(font, text, position, align, 1, paletteSource.Palette[color], spacing, paragraphSpacing, outlineClr, context);;
    }
}