using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Values;

namespace Ssit.CrossX.UI.Views;

public class ScrollView: Background, IFocusableView
{
    public View ContentView { get; set; }
    public ScrollMode ScrollMode { get; set; }
    public Length? ManualScrollSpeed { get; set; }
    
    public RgbaColor? FocusedBackgroundColor { get; set; }
    
    public string UniqueId { get; set; }
    public (string left, string right) HorizontalNavigation { get; set; }
    public (string up, string down) VerticalNavigation { get; set; }
}