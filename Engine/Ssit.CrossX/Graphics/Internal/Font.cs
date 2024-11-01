using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Ssit.CrossX.IO;
using Ssit.CrossX.IoC;
using Ssit.CrossX.Utils;

namespace Ssit.CrossX.Graphics.Internal;

internal class Font : IFont
{
    public delegate void DrawTextureDelegate(ITexture texture, Vector2 position, Rectangle source, RgbaColor color);

    private struct TextSource
    {
        public StringBuilder Builder;
        public string String;

        public char this[int index] => String is not null ? String[index] : Builder[index];
        public int Length => String?.Length ?? Builder.Length;
    }

    public class FontInfo
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Kerning { get; set; }
        public bool TechnicalLine { get; set; }
        public int OutlineColor { get; set; }
    }

    private ITexture FillSheet { get; }
    private ITexture OutlineSheet { get; }
    public IReadOnlyList<Rectangle> Glyphs => _glyphs;
    private int Kerning { get; }
    
    public string Name { get; }
    public int Size { get; }
    private int WhiteSpaceWidth { get; }
    
    private readonly Rectangle[] _glyphs;
    
    internal Font(FontInfo fontInfo, IIoCContainer container, IFilesProvider filesProvider)
    {
        using var stream = filesProvider.Open(fontInfo.Path);
        var colors = ImagesLoader.LoadImage(stream);

        var outlineColor = new RgbaColor(fontInfo.OutlineColor);
        
        var width = colors.GetLength(0);
        var height = colors.GetLength(1);
        
        var outlineColors = new RgbaColor[width, height];
        var fillColors = new RgbaColor[width, height];
        
        bool hasOutline = false;

        for (var x = 0; x < width; ++x)
        {
            for (var y = 0; y < height; ++y)
            {
                if (colors[x, y] != outlineColor)
                {
                    fillColors[x, y] = colors[x,y];
                }

                if (colors[x, y] == outlineColor)
                {
                    outlineColors[x, y] = RgbaColor.White;
                    hasOutline = true;
                }
            }
        }

        if (hasOutline)
        {
            OutlineSheet = container.IoCConstruct<ITexture>(outlineColors);
        }
        FillSheet = container.IoCConstruct<ITexture>(fillColors);
        
        Name = fontInfo.Name;
        
        Kerning = fontInfo.Kerning;
        Size = fontInfo.Height;
        WhiteSpaceWidth = fontInfo.Width;

        if (fontInfo.TechnicalLine)
        {
            Size--;
        }

        _glyphs = LoadGlyphs(fontInfo, colors);
    }

    internal void DrawText(DrawTextureDelegate drawDelegate, string text, Vector2 position, RgbaColor color, RgbaColor? outlineColor = null)
    {
        DrawText(new TextSource
        {
            String = text
        }, position, color, FillSheet, drawDelegate);

        if (OutlineSheet is not null && outlineColor.HasValue)
        {
            DrawText(new TextSource
            {
                String = text
            }, position, outlineColor.Value, OutlineSheet, drawDelegate);
        }
    }

    internal void DrawText(DrawTextureDelegate drawDelegate, StringBuilder text, Vector2 position, RgbaColor color, RgbaColor? outlineColor = null)
    {
        DrawText(new TextSource
        {
            Builder = text
        }, position, color, FillSheet, drawDelegate);

        if (OutlineSheet is not null && outlineColor.HasValue)
        {
            DrawText(new TextSource
            {
                Builder = text
            }, position, outlineColor.Value , OutlineSheet, drawDelegate);
        }
    }
    
    private void DrawText(TextSource text, Vector2 position, RgbaColor color, ITexture sheet, DrawTextureDelegate drawDelegate)
    {
        var px = (int)position.X;
        var py = (int)position.Y;
        
        var startX = px;

        for (var idx = 0; idx < text.Length; ++idx)
        {
            if (text[idx] == '\n')
            {
                px = startX;
                py += Size;
                continue;
            }
            
            int characterWidth = WhiteSpaceWidth;
            var index = text[idx] - 33;
            if (index >= 0 && index < _glyphs.Length)
            {
                characterWidth = _glyphs[index].Width;
                drawDelegate(sheet, position, _glyphs[index], color);
            }

            px += characterWidth + Kerning;
        }
    }

    public Size TextSize(string text) =>
        TextSize(new TextSource
        {
            String = text
        });

    public Size TextSize(StringBuilder text) =>
        TextSize(new TextSource
        {
            Builder = text
        });

    private Size TextSize(TextSource text)
    {
        int width = 0;

        int maxWidth = 0;
        int lines = 0;
        
        for (var idx = 0; idx < text.Length; ++idx)
        {
            if (text[idx] == '\n')
            {
                maxWidth = Math.Max(width, maxWidth);
                width = 0;
                lines++;
                continue;
            }
            var index = text[idx] - 33;
            int characterWidth = WhiteSpaceWidth;
            
            if (index >= 0 && index < _glyphs.Length)
            {
                characterWidth = _glyphs[index].Width;
            }
            
            if (idx < text.Length - 1)
            {
                characterWidth += Kerning;
            }

            width += characterWidth;
        }

        if (width > 0) lines++;
        
        maxWidth = Math.Max(width, maxWidth);
        return new Size(maxWidth, lines * Size);
    }

    private Rectangle[] LoadGlyphs(FontInfo fontInfo, RgbaColor[,] pixels)
    {
        var list = new List<Rectangle>();

        var startX = -1;

        var lines = pixels.GetLength(1) / fontInfo.Height;
        var height = fontInfo.Height;
        var width = pixels.GetLength(0);

        var targetHeight = height - (fontInfo.TechnicalLine ? 1 : 0);
        
        for (var line = 0; line < lines; ++line)
        {
            var startY = line * fontInfo.Height;

            for (var x = 0; x < width; ++x)
            {
                bool cleanLine = true;
                for (var y = 0; y < height; ++y)
                {
                    var py = startY + y;

                    if (pixels[x, py].A > 0)
                    {
                        cleanLine = false;
                        break;
                    }
                }

                if (!cleanLine && startX < 0)
                {
                    startX = x;
                }
                else if (cleanLine && startX >= 0)
                {
                    list.Add(new Rectangle(startX, startY, x - startX, targetHeight));
                    startX = -1;
                }
            }
        }

        return list.ToArray();
    }

    public void Dispose()
    {
        FillSheet?.Dispose();
        OutlineSheet?.Dispose();
    }
}