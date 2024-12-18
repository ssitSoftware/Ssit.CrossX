using Ssit.CrossX.Graphics;
using Ssit.CrossX.UI.Values;

namespace Ssit.CrossX.UI.Views;

public class Label: Background
{
    public SharedString Text { get; set; }
    
    public RgbaColor? TextColor { get; set; }
    public RgbaColor? TextOutlineColor { get; set; }
    
    public ContentAlign? TextAlign { get; set; }
    public TextSpacing? TextSpacing { get; set; }
    
    public FontDesc? Font { get; set; }
    
    public Thickness? Padding { get; set; }
}