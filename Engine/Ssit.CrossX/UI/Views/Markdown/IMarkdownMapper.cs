using Ssit.CrossX.UI.Values;

namespace Ssit.CrossX.UI.Views.Markdown;

public interface IMarkdownMapper
{
    FontDesc GetFont(MarkdownStyle style);
    string GetImagePath(string path);
}