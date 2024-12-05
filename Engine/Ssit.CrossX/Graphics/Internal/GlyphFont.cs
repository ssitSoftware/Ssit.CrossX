using System;
using System.Collections.Generic;
using System.IO;

namespace Ssit.CrossX.Graphics.Internal;

public class GlyphFont
{
    public class FontMetrics(int capHeight, int xHeight, int ascender, int descender, int lineHeight, int whitespaceWidth)
    {
        public int CapHeight { get; } = capHeight;
        public int XHeight { get; } = xHeight;
        public int Ascender { get; } = ascender;
        public int Descender { get; } = descender;
        public int LineHeight { get; } = lineHeight;
        public int WhitespaceWidth { get; } = whitespaceWidth;

        internal void Save(BinaryWriter writer)
        {
            writer.Write(CapHeight);
            writer.Write(XHeight);
            writer.Write(Ascender);
            writer.Write(Descender);
            writer.Write(LineHeight);
            writer.Write(WhitespaceWidth);
        }

        internal static FontMetrics Load(BinaryReader reader)
        {
            var capHeight = reader.ReadInt32();
            var xHeight = reader.ReadInt32();
            var ascender = reader.ReadInt32();
            var descender = reader.ReadInt32();
            var lineHeight = reader.ReadInt32();
            var whitespaceWidth = reader.ReadInt32();
            
            return new FontMetrics(capHeight, xHeight, ascender, descender, lineHeight, whitespaceWidth);
        }
    }
    
    public string Name { get; private set; }
    public int Size { get; private set; }
    public FontMetrics Metrics { get; private set; }
    
    private const int FirstCharacter = 32;
    private const int NumBasicCharacters = 128 - FirstCharacter;
    
    private readonly Glyph[] _glyphs = new Glyph[NumBasicCharacters];
    private readonly Dictionary<char, Glyph> _extendedGlyphs = new();

    protected GlyphFont()
        : this("", 0, null)
    {
    }
    
    private GlyphFont(string name, int size, FontMetrics metrics)
    {
        Name = name;
        Size = size;
        Metrics = metrics;
    }
    
    public GlyphFont(string name, int size, FontMetrics metrics, IReadOnlyList<Glyph> glyphs)
        : this(name, size, metrics)
    {
        foreach (var glyph in glyphs)
        {
            var index = glyph.Char - FirstCharacter;

            if (index is >= 0 and < NumBasicCharacters)
            {
                _glyphs[index] = glyph;
            }
            else
            {
                _extendedGlyphs.Add(glyph.Char, glyph);
            }
        }
    }

    public void Load(Stream stream)
    {
        var reader = new BinaryReader(stream);
        
        Name = reader.ReadString();
        Size = reader.ReadInt32();
        Metrics = FontMetrics.Load(reader);
        
        var glyph = Glyph.Read(reader);
        while (glyph is not null)
        {
            var index = glyph.Char - FirstCharacter;
            if (index is >= 0 and < NumBasicCharacters)
            {
                _glyphs[index] = glyph;
            }
            else
            {
                _extendedGlyphs.Add(glyph.Char, glyph);
            }
            glyph = Glyph.Read(reader);
        }
    }

    public void Save(Stream stream)
    {
        var writer = new BinaryWriter(stream);
        
        writer.Write(Name);
        writer.Write(Size);
        Metrics.Save(writer);

        for (var idx = 0; idx < _glyphs.Length; idx++)
        {
            var glyph = _glyphs[idx];
            var c = (char)(idx + FirstCharacter);

            if (glyph is null)
                continue;
            
            glyph.Save(writer);
        }
        
        foreach (var glyph in _extendedGlyphs)
        {
            glyph.Value.Save(writer);
        }
        writer.Write((char)0);
        writer.Flush();
    }
    
    public Glyph GetGlyph(char c)
    {
        var index = c - FirstCharacter;
        
        if (index < 0) return null;
        
        if (index < NumBasicCharacters)
        {
            return _glyphs[index];
        }

        return _extendedGlyphs.GetValueOrDefault(c, null);
    }
    
    protected Size TextSize(TextSource text)
    {
        float width = 0;

        float maxWidth = 0;
        int lines = 0;

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
            var glyph = GetGlyph(c);
            
            float advance = Metrics.WhitespaceWidth;
            
            if (glyph != null)
            {
                advance = glyph.Advance + glyph.GetKerning(previousCharacter);
                previousCharacter = c;
            }
            else
            {
                previousCharacter = '\0';
            }

            width += advance;
        }

        if (width > 0) lines++;
        
        maxWidth = Math.Max(width, maxWidth);
        return new Size((int)MathF.Ceiling(maxWidth), lines * Size);
    }
}