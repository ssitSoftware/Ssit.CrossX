using System.Collections.Generic;
using Ssit.CrossX.UI.Values;

namespace Ssit.CrossX.UI.Views.Markdown;

public class FontMapper: IMarkdownFontMapper
{
    private readonly Dictionary<MarkdownStyle, FontDesc> _mappings = new();
    private FontDesc _defaultFont = ("Default", 12);

    public FontMapper With(MarkdownStyle style, FontDesc font)
    {
        _mappings.Add(style, font);
        return this;
    }

    public FontMapper WithDefault(FontDesc font)
    {
        _defaultFont = font;
        return this;
    }
    
    public FontDesc GetFont(MarkdownStyle style) => _mappings.GetValueOrDefault(style, _defaultFont);
}