using Ssit.CrossX.Input;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Values;

namespace Ssit.CrossX.UI.Views;

public class TextInput: View
{
    public FontDesc? Font { get; set; }
    
    public SharedString Text { get; set; }
    public SharedString Placeholder { get; set; }
    
    public InputType? InputType { get; set; }
    
    public Thickness? Padding { get; set; }
    
    public ButtonStateColors BackgroundColors { get; set; }
    
    public ButtonStateColors PlaceholderColors { get; set; }
    public ButtonStateColors PlaceholderOutlineColors { get; set; }
    
    public ButtonStateColors TextColors { get; set; }
    public ButtonStateColors TextOutlineColors { get; set; }
    
    public ButtonStateColors FrameColors { get; set; }
    public ColorWrapper? ActiveFrameColor { get; set; }
    
    public Length? FrameThickness { get; set; }
    public Length? ActiveFrameThickness { get; set; }
    public string UniqueId { get; set; }
    public TextScaling Scaling { get; set; } = TextScaling.Default;
    public SharedBool Enabled { get; set; }
}