using System.Windows.Input;
using Ssit.CrossX.UI.Values;

namespace Ssit.CrossX.UI.Views;

public class Button: View
{
    public SharedString Text { get; set; }
    
    public RgbaColor? Color { get; set; }
    public RgbaColor? SelectedColor { get; set; }
    public RgbaColor? PushedColor { get; set; }
    public RgbaColor? DisabledColor { get; set; }
    
    public ICommand Command { get; set; }
}