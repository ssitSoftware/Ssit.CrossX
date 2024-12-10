using Ssit.CrossX.Graphics;
using Ssit.CrossX.UI.Values;

namespace Ssit.CrossX.UI.Views;

public class Label: View
{
    public SharedString Text { get; set; }
    
    public RgbaColor? TextColor { get; set; }
    public RgbaColor? TextOutlineColor { get; set; }
    
    public TextAlign? TextAlign { get; set; }
    public TextSpacing? TextSpacing { get; set; }
}