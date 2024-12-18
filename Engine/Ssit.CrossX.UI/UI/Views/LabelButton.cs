using System.Windows.Input;

namespace Ssit.CrossX.UI.Views;

public class LabelButton: Label
{
    public RgbaColor? DisabledTextColor { get; set; }
    public RgbaColor? DisabledTextOutlineColor { get; set; }
    public RgbaColor? DisabledBackgroundColor { get; set; }
    
    public RgbaColor? HoverTextColor { get; set; }
    public RgbaColor? HoverTextOutlineColor { get; set; }
    public RgbaColor? HoverBackgroundColor { get; set; }
    
    public RgbaColor? FocusedTextColor { get; set; }
    public RgbaColor? FocusedTextOutlineColor { get; set; }
    public RgbaColor? FocusedBackgroundColor { get; set; }
    
    public ICommand Command { get; set; }
    public object CommandParameter { get; set; }
    
    public string UniqueId { get; set; }
    
    public (string left, string right) HorizontalNavigation { get; set; }
    public (string up, string down) VerticalNavigation { get; set; }
}