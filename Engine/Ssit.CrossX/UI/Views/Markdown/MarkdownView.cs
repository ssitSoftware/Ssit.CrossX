using Ssit.CrossX.UI.Values;
using Ssit.CrossX.UI.Views.Markdown;

namespace Ssit.CrossX.UI.Views;

public class MarkdownView: Background
{
    public SharedString Text { get; set; }
    
    public ColorWrapper? TextColor { get; set; }
    public ColorWrapper? TextOutlineColor { get; set; }
    public TextScaling Scaling { get; set; }
    
    public IMarkdownFontMapper FontMapper { get; set; }
}