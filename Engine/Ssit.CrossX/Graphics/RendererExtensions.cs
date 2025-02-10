using System;
using Ssit.CrossX.Graphics.Font;
using Ssit.CrossX.Graphics.Internal;
using Ssit.CrossX.Text;

namespace Ssit.CrossX.Graphics;

public static class RendererExtensions
{
    public static TextRenderingContext CalculateMultilineText(this IFont font, TextSource text, TextSpacing spacing, float maxWidth, float paragraphSpacing, TextRenderingContext context = null)
    {
        if(font is not IGlyphFont glyphFont) throw new NotSupportedException("This kind of font is not supported");

        context ??= new TextRenderingContext();
        
        GlyphFontRenderer.CalculateMultilineText(glyphFont, text, maxWidth, spacing, paragraphSpacing, context);
        return context;
    }
    
    public static TextRenderingContext CalculateText(this IFont font, TextSource text, TextSpacing spacing, TextRenderingContext context = null)
    {
        if(font is not IGlyphFont glyphFont) throw new NotSupportedException("This kind of font is not supported");

        context ??= new TextRenderingContext();
        
        GlyphFontRenderer.CalculateText(glyphFont, text, spacing, context);
        return context;
    }
}