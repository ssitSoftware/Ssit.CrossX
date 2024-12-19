using System;
using System.Collections.Generic;
using Ssit.CrossX.Text;

namespace Ssit.CrossX.Graphics;

public class TextRenderingContext
{
    public struct LineDefinition
    {
        public float Width;
        public TextSource Text;
        public int Whitespaces;
        public bool EndOfParagraph;
        public float Spacing;
    }

    private List<char> _text;
    private IGlyphFont _font;
    private TextSpacing _spacing;
    private int _textHashCode = -1;
    private int _targetWidth = 0;
    
    public List<LineDefinition> Lines { get; } = new ();

    private float _width;
    private float _height;
    private bool _fullStringCheckEnabled;

    internal IGlyphFont Font => _font;
    
    public float TargetWidth => _targetWidth;
    
    public float Width
    {
        get
        {
            lock (this)
            {
                if (_width < 0)
                {
                    var width = 0f;
                    for (int i = 0; i < Lines.Count; i++)
                    {
                        width = MathF.Max(Lines[i].Width, width);
                    }

                    _width = width;
                }

                return _width;
            }
        }
    }
    
    public float Height
    {
        get
        {
            lock (this)
            {
                if (_height < 0)
                {
                    float height = Lines.Count * _font?.Metrics?.LineHeight ?? 0;
                    for (int idx = 0; idx < Lines.Count; idx++)
                    {
                        height += Lines[idx].Spacing;
                    }

                    _height = height;
                }

                return _height;
            }
        }
    }

    public bool FullStringCheckEnabled
    {
        get => _fullStringCheckEnabled;
        set
        {
            _fullStringCheckEnabled = value;
            if (!_fullStringCheckEnabled)
            {
                _text = null;
            }
        }
    }

    public void Update(TextSource text, IGlyphFont font, TextSpacing spacing, int targetWidth = 0)
    {
        if (FullStringCheckEnabled)
        {
            _text ??= new List<char>();
            _text.Clear();
            
            for (var idx = 0; idx < text.Length; idx++)
            {
                _text.Add(text[idx]);
            }
        }

        _targetWidth = targetWidth;
        _font = font;
        Lines.Clear();
        _spacing = spacing;
        _width = -1;
        _height = -1;

        _textHashCode = text.GetHashCode();
    }

    private bool IsTextEqual(TextSource text)
    {
        if (_text is null)
        {
            return false;
        }
        
        if (text.Length != _text.Count)
        {
            return false;
        }
        
        for(var idx =0; idx < text.Length; idx++)
        {
            if (text[idx] != _text[idx])
            {
                return false;
            }
        }

        return true;
    }

    public bool IsValid(TextSource text, IGlyphFont font, TextSpacing spacing, int targetWidth = 0)
    {
        if (_targetWidth != targetWidth)
        {
            if(_targetWidth != 0) return false;

            if (targetWidth > 0 && Width > targetWidth + font.Metrics.WhitespaceWidth)
            {
                return false;
            }
        }
        
        if (spacing != _spacing)
        {
            return false;
        }
        
        if (!ReferenceEquals(font, _font))
        {
            return false;
        }

        if (_textHashCode != text.GetHashCode())
        {
            return false;
        }
        
        if (FullStringCheckEnabled && !IsTextEqual(text))
        {
            return false;
        }

        return true;
    }
}