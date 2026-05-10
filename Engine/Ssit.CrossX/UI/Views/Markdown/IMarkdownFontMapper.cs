using Ssit.CrossX.UI.Values;

namespace Ssit.CrossX.UI.Views.Markdown;

public interface IMarkdownFontMapper
{
    FontDesc GetFont(MarkdownStyle style);
}