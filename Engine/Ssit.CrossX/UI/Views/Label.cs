using Ssit.CrossX.Graphics;
using Ssit.CrossX.UI.Values;

namespace Ssit.CrossX.UI.Views;

public enum TextScaling
{
    None,
    Default,
    Pixel
}

public class Label: Background
{
    public SharedString Text { get; set; }
    public ColorWrapper TextColor { get; set; }
    public ColorWrapper TextOutlineColor { get; set; }
    
    public ContentAlign? TextAlign { get; set; }
    public TextSpacing? TextSpacing { get; set; }
    
    public FontDesc? Font { get; set; }
    
    public Thickness? Padding { get; set; }
    public TextScaling Scaling { get; set; }
}