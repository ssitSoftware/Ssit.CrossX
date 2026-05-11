using System.Collections.Generic;

namespace Ssit.CrossX.UI.Handlers.Markdown;

internal class MarkdownBlock
{
    public MarkdownBlockType Type;
    public readonly List<InlineSpan> Spans = new();
    public string ImagePath;
    public float MarginBottom;
}