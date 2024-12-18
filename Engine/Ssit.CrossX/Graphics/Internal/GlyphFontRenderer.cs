using System;
using System.Collections.Generic;
using System.Numerics;
using Ssit.CrossX.Text;

namespace Ssit.CrossX.Graphics.Internal;

public delegate void DrawTextureQuadDelegate(ITexture texture,
    Rectangle target, Rectangle source, RgbaColor color, float depth = 0);

internal static class GlyphFontRenderer
{
    private static readonly TextRenderingContext TempContext = new();
    
    public static void RenderText(IRenderer renderer, IGlyphFont font, TextSource text, Vector2 position, ContentAlign align,
        RgbaColor color, RgbaColor outlineColor, TextSpacing spacing, float depth, TextRenderingContext context)
    {
        if (context is null)
        {
            context = TempContext;
            CalculateLines(font, text, spacing, context);
        }
        else if(!context.IsValid(text, font, spacing))
        {
            CalculateText(font, text, spacing, context);
        }

        var internalRenderer = renderer.Unsafe;
        if (internalRenderer is null) return;
        
        if (font.OutlineSheet is not null && outlineColor.A > 0)
        {
            internalRenderer?.BeginRender(font.OutlineSheet, TextureFilter.Nearest);
            RenderText(internalRenderer.FastDrawQuad, font.OutlineSheet, font, context.Lines, position, align,
                outlineColor, spacing, context.Width, context.Height, depth);
        }

        if (color.A > 0)
        {
            internalRenderer?.BeginRender(font.FontSheet, TextureFilter.Nearest);
            RenderText(internalRenderer.FastDrawQuad, font.FontSheet, font, context.Lines, position, align, color,
                spacing, context.Width, context.Height, depth);
        }
    }

    public static void RenderText(IRenderer renderer, IGlyphFont font, TextSource text, RectangleF target, ContentAlign align,
        RgbaColor color, RgbaColor outlineColor, TextSpacing spacing, float paragraphSpacing, float depth, TextRenderingContext context)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context), "TextRenderingContext must be not null!");
        }
        
        if (!context.IsValid(text, font, spacing, (int)target.Width))
        {
            CalculateMultilineText(font, text, target.Width, spacing, paragraphSpacing, context);
        }

        var position = target.TopLeft;

        if ((align & ContentAlign.Center) == ContentAlign.Center)
        {
            position.X = target.Center.X;
        }
        
        if ((align & ContentAlign.VCenter) == ContentAlign.VCenter)
        {
            position.Y = target.Center.Y;
        }
        
        if ((align & ContentAlign.Right) == ContentAlign.Right)
        {
            position.X = target.Right;
        }
        
        if ((align & ContentAlign.Bottom) == ContentAlign.Bottom)
        {
            position.Y = target.Bottom;
        }

        var internalRenderer = renderer.Unsafe;
        if (internalRenderer is null) return;
        
        if (font.OutlineSheet is not null && outlineColor.A > 0)
        {
            internalRenderer?.BeginRender(font.OutlineSheet, TextureFilter.Nearest);
            RenderText(internalRenderer.FastDrawQuad, font.OutlineSheet, font, context.Lines, position, align,
                outlineColor, spacing, context.Width, context.Height, depth);
        }

        if (color.A > 0)
        {
            internalRenderer?.BeginRender(font.FontSheet, TextureFilter.Nearest);
            RenderText(internalRenderer.FastDrawQuad, font.FontSheet, font, context.Lines, position, align, color,
                spacing, context.Width, context.Height, depth);
        }
    }

    public static void CalculateText(IGlyphFont font, TextSource text, TextSpacing spacing, TextRenderingContext context)
    {
        if (!context.IsValid(text, font, spacing))
        {
            context.Update(text, font, spacing);
            CalculateLines(font, text, spacing, context);
        }
    }

    public static void CalculateMultilineText(IGlyphFont font, TextSource text, float targetWidth,
        TextSpacing spacing, float paragraphSpacing, TextRenderingContext context)
    {
        if (context.IsValid(text, font, spacing, (int)targetWidth))
        {
            return;
        }
        
        context.Update(text, font, spacing, (int)targetWidth);
        CalculateWrapLines(font, text, spacing, paragraphSpacing, context, (int)targetWidth);
    }

    private static void CalculateWrapLines(IGlyphFont font, TextSource text, TextSpacing spacing, float paragraphSpacing, TextRenderingContext context, int maxWidth)
    {
        context.Lines.Clear();

        var start = 0;
        var lastSmaller = 0;

        var newLineSpacing = 0f;
        
        for (var idx = 0; idx < text.Length; ++idx)
        {
            if (text[idx] == '\n')
            {
                var line = GetLine(text, start, idx - start, font, spacing);
                line.Spacing = newLineSpacing;
                line.EndOfParagraph = true;
                newLineSpacing = paragraphSpacing;
                start = idx + 1;
                lastSmaller = start;
                context.Lines.Add(line);
                continue;
            }

            if (font.GetGlyph(text[idx]) == null)
            {
                var width = MeasureWidth(font, new TextSource(text, start, idx - start), spacing);
                if (width <= maxWidth)
                {
                    lastSmaller = idx;
                }
                else
                {
                    if (lastSmaller > start)
                    {
                        var line = GetLine(text, start, lastSmaller - start, font, spacing);
                        line.Spacing = newLineSpacing;
                        newLineSpacing = 0;
                        start = lastSmaller + 1;
                        idx = start;
                        context.Lines.Add(line);
                    }
                    else
                    {
                        var line = GetLine(text, start, idx - start, font, spacing);
                        line.EndOfParagraph = true;
                        line.Spacing = newLineSpacing;
                        newLineSpacing = 0;
                        start = idx + 1;
                        lastSmaller = start;
                        context.Lines.Add(line);
                    }
                }
            }
        }
        
        var line3 = GetLine(text, start, text.Length - start, font, spacing);
        line3.EndOfParagraph = true;
        line3.Spacing = newLineSpacing;
        context.Lines.Add(line3);
    }

    internal static void RenderText(DrawTextureQuadDelegate drawDelegate, ITexture texture, IGlyphFont font, IReadOnlyList<TextRenderingContext.LineDefinition> lines, 
        Vector2 position, ContentAlign align, RgbaColor color, TextSpacing spacing, float justifyWidth, float height, float depth)
    {
        float additionalSpacing = (int)(spacing - 50) * font.Metrics.WhitespaceWidth / 50f;
        
        var posY = CalculateYPosition(font, position.Y, (int)MathF.Ceiling(height), align, lines.Count == 1);

        for (var idx = 0; idx < lines.Count; ++idx)
        {
            var line = lines[idx];
            var posX = CalculateXPosition(position.X, align, line.Width);
            posY += line.Spacing;

            var whitespaceSize = font.Metrics.WhitespaceWidth + additionalSpacing;

            if ((align & ContentAlign.Justified) == ContentAlign.Justified && line is { Whitespaces: > 0, EndOfParagraph: false })
            {
                var lineWidthNoSpaces = line.Width - line.Whitespaces * whitespaceSize;
                whitespaceSize = (justifyWidth - lineWidthNoSpaces) / line.Whitespaces;
            }
            
            var previousCharacter = '\0';
            for(var i = 0; i < line.Text.Length; ++i)
            {
                var c = line.Text[i];
                var glyph = font.GetGlyph(c);
             
                if (glyph == null)
                {
                    posX += whitespaceSize;
                    previousCharacter = '\0';
                    continue;
                }
                
                posX += glyph.GetKerning(previousCharacter);
            
                var target = new Rectangle( (int)(posX + glyph.Offset.X), (int)(posY + glyph.Offset.Y), glyph.Source.Width, glyph.Source.Height);
                drawDelegate(texture, target, glyph.Source, color: color, depth: depth);

                posX += glyph.Advance + additionalSpacing;
                previousCharacter = c;
            }
            posY += font.Metrics.LineHeight;
        }
    }

    private static float CalculateXPosition(float positionX, ContentAlign align, float lineWidth)
    {
        if ((align & ContentAlign.Center) == ContentAlign.Center)
        {
            return MathF.Ceiling(positionX - lineWidth / 2f);
        }

        if ((align & ContentAlign.Right) == ContentAlign.Right)
        {
            return positionX - lineWidth;
        }

        return positionX;
    }

    private static float CalculateYPosition(IGlyphFont font, float positionY, int height, ContentAlign align, bool oneLine)
    {
        if ((align & ContentAlign.VCenter) == ContentAlign.VCenter)
        {
            var offset = font.Metrics.XHeight + font.Metrics.Ascender;
            
            return MathF.Ceiling(positionY - (height / 2f - font.Metrics.LineHeight / 2f) + font.Metrics.XHeight / 2f);
        }
        
        if ((align & ContentAlign.Bottom) == ContentAlign.Bottom)
        {
            return positionY - height - font.Metrics.Ascender;
        }

        return positionY - font.Metrics.Ascender;
    }

    private static void CalculateLines(IGlyphFont font, TextSource text, TextSpacing spacing, TextRenderingContext context)
    {
        context.Lines.Clear();
        var length = text.Length;
        var position = 0;
        
        for (var idx = 0; idx < length; ++idx)
        {
            if (text[idx] != '\n') continue;
            
            var line = GetLine(text, position, idx - position, font, spacing);
            context.Lines.Add(line);
            position = idx + 1;
        }

        var lastLine = GetLine(text, position, length - position, font, spacing);
        context.Lines.Add(lastLine);
    }

    private static TextRenderingContext.LineDefinition GetLine(TextSource source, int start, int length, IGlyphFont font, TextSpacing spacing)
    {
        var line = new TextSource(source, start, length);
        var width = MeasureWidth(font, line, spacing);
        var whitespaces = 0;
                
        for (var i = 0; i < line.Length; ++i)
        {
            if (font.GetGlyph(line[i]) is null)
            {
                whitespaces++;
            }
        }

        return new TextRenderingContext.LineDefinition
        {
            Text = line,
            Width = width,
            Whitespaces = whitespaces,
            EndOfParagraph = false
        };
    }
    
    private static float MeasureWidth(IGlyphFont glyphFont, TextSource text, TextSpacing spacing)
    {
        float width = 0;
        float additionalSpacing = (int)(spacing - 50) * glyphFont.Metrics.WhitespaceWidth / 50f;

        char previousCharacter = '\0';
        
        for (var idx = 0; idx < text.Length; ++idx)
        {
            var c = text[idx];
            var glyph = glyphFont.GetGlyph(c);

            float advance = glyphFont.Metrics.WhitespaceWidth + additionalSpacing;
            
            if (glyph != null)
            {
                advance = glyph.Advance + glyph.GetKerning(previousCharacter) + additionalSpacing;
                previousCharacter = c;
            }
            else
            {
                previousCharacter = '\0';
            }

            width += advance;
        }

        return width;
    }
    
    public static Size MeasureText(IGlyphFont glyphFont, TextSource text, TextSpacing spacing)
    {
        float width = 0;

        float maxWidth = 0;
        int lines = 0;
        
        float additionalSpacing = (int)(spacing - 50) * glyphFont.Metrics.WhitespaceWidth / 50f;

        char previousCharacter = '\0';
        
        for (var idx = 0; idx < text.Length; ++idx)
        {
            if (text[idx] == '\n')
            {
                maxWidth = Math.Max(width, maxWidth);
                width = 0;
                lines++;
                previousCharacter = '\0';
                continue;
            }
            var c = text[idx];
            var glyph = glyphFont.GetGlyph(c);

            float advance = glyphFont.Metrics.WhitespaceWidth + additionalSpacing;
            
            if (glyph != null)
            {
                advance = glyph.Advance + glyph.GetKerning(previousCharacter) + additionalSpacing;
                previousCharacter = c;
            }
            else
            {
                previousCharacter = '\0';
            }

            width += advance;
        }

        lines++;
        
        maxWidth = Math.Max(width, maxWidth);
        return new Size((int)MathF.Ceiling(maxWidth), lines * glyphFont.Metrics.LineHeight);
    }
}