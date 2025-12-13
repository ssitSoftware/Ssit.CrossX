using System.Numerics;
using Ssit.CrossX.Graphics.Font;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.Text;

namespace Ssit.CrossX.Graphics;

internal class SmartTextRenderer(IFontsManager fontsManager, IRenderer2 renderer2): ISmartTextRenderer
{
    private string _fontName;
    private float _size;

    private TextRenderingContext _context = new();
    
    public void PrepareFont(string fontName, float size)
    {
        _fontName = fontName;
        _size = size;
    }
    
    public void DrawText(TextSource text, Vector2 position, ContentAlign align = ContentAlign.Left,
        float scale = 1, RgbaColor? color = null,
        TextSpacing spacing = TextSpacing.Normal, RgbaColor? outlineColor = null, TextRenderingContext context = null)
    {
        var font =  fontsManager.GetFont(_fontName, _size * renderer2.StateProvider.Scale);
        scale /= renderer2.StateProvider.Scale;
        renderer2.TextRenderer.DrawText(font, text, position, align, scale, color, spacing, outlineColor, context);
    }

    public void DrawText(TextSource text, RectangleF position, ContentAlign align = ContentAlign.Left,
        float scale = 1, RgbaColor? color = null,
        TextSpacing spacing = TextSpacing.Normal, float paragraphSpacing = -1, RgbaColor? outlineColor = null,
        TextRenderingContext context = null)
    {
        var font =  fontsManager.GetFont(_fontName, _size * renderer2.StateProvider.Scale);
        scale /= renderer2.StateProvider.Scale;
        renderer2.TextRenderer.DrawText(font, text, position, align, scale, color, spacing, paragraphSpacing, outlineColor, context);
    }
}