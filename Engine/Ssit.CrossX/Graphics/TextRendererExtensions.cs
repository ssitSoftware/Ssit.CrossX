using System;
using Ssit.CrossX.Graphics.Internal;
using Ssit.CrossX.Text;

namespace Ssit.CrossX.Graphics;

public static class TextRendererExtensions
{
    public static TextRenderingContext CalculateMultilineText(this IFont font, TextSource text,
        TextAlign align, TextSpacing spacing, float maxWidth, float paragraphSpacing, TextRenderingContext context = null)
    {
        if(font is not IGlyphFont glyphFont) throw new NotSupportedException("This kind of font is not supported");

        context ??= new TextRenderingContext();
        
        GlyphFontRenderer.CalculateMultilineText(glyphFont, text, maxWidth, align, spacing, paragraphSpacing, context);
        return context;
    }
}