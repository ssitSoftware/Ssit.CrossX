using System.Numerics;

namespace Ssit.CrossX.Graphics.Internal;

public static class GlyphFontRenderer
{
    public static void RenderText(IRenderer renderer, IGlyphFont font, TextSource text, Vector2 position,
        RgbaColor color, RgbaColor outlineColor, TextSpacing spacing, float depth)
    {
        if (font.OutlineSheet is not null)
        {
            RenderText(renderer, font.OutlineSheet, font, text, position, outlineColor, spacing, depth);
        }
        
        RenderText(renderer, font.FontSheet, font, text, position, color, spacing, depth);
    }
    
    private static void RenderText(IRenderer renderer, ITexture texture, IGlyphFont font, TextSource text, Vector2 position, RgbaColor color, TextSpacing spacing, float depth)
    {
        Vector2 pos = position;

        float additionalSpacing = (int)(spacing - 50) * font.Metrics.WhitespaceWidth / 50f;
        
        char previousCharacter = '\0';
        for (var idx = 0; idx < text.Length; ++idx)
        {
            var c = text[idx];

            if (c == '\n')
            {
                pos.X = position.X;
                pos.Y += font.Metrics.LineHeight;
                previousCharacter = '\0';
                continue;
            }
            
            var glyph = font.GetGlyph(c);

            if (glyph == null)
            {
                pos.X += font.Metrics.WhitespaceWidth + additionalSpacing;
                previousCharacter = '\0';
                continue;
            }

            pos.X += glyph.GetKerning(previousCharacter);
            
            renderer.DrawTexture(texture, pos + glyph.Offset, glyph.Source, color: color, depth: depth);

            pos.X += glyph.Advance + additionalSpacing;
            previousCharacter = c;
        }
    }
}