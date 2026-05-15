using System.Collections.Generic;

namespace Ssit.CrossX.UI.Handlers.Markdown;

// This class was created with Claude Code assistance
internal class MarkdownBlock
{
    public MarkdownBlockType Type;
    public readonly List<InlineSpan> Spans = new();
    public string ImagePath;
    public float MarginBottom;
}