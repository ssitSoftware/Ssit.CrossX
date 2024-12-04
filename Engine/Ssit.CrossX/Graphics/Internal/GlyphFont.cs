using System;
using System.Collections.Generic;
using System.IO;

namespace Ssit.CrossX.Graphics.Internal;

public class GlyphFont
{
    public int WhiteSpaceWidth { get; set; }
    public string Name { get; private set; }
    public int Size { get; private set; }
    
    private const int FirstCharacter = 32;
    private const int NumBasicCharacters = 128 - FirstCharacter;
    
    private readonly Glyph[] _glyphs = new Glyph[NumBasicCharacters];
    private readonly Dictionary<char, Glyph> _extendedGlyphs = new();

    protected GlyphFont() 
        : this("", 0, 0)
    {
    }
    
    private GlyphFont(string name, int size, int whiteSpaceWidth)
    {
        Name = name;
        Size = size;
        WhiteSpaceWidth = whiteSpaceWidth;
    }
    
    public GlyphFont(string name, int size, int whiteSpaceWidth, IReadOnlyList<Glyph> glyphs)
        : this(name, size, whiteSpaceWidth)
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
        WhiteSpaceWidth = reader.ReadInt32();
        
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
        writer.Write(WhiteSpaceWidth);

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
    
    private Glyph GetGlyph(char c)
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
            
            float advance = WhiteSpaceWidth;
            
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