using System;
using Ssit.CrossX.Input;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Values;

namespace Ssit.CrossX.UI.Views;

[Flags]
public enum TextUpdateMode
{
    Accept =  0,
    Unfocus = 1,
    Live = 2
}

public class TextInput: View
{
    public FontDesc? Font { get; set; }
    
    public SharedStringValue Text { get; set; }
    public SharedString Placeholder { get; set; }
    
    public InputType? InputType { get; set; }
    
    public Thickness? Padding { get; set; }
    
    public IButtonStateColors BackgroundColors { get; set; }
    
    public IButtonStateColors PlaceholderColors { get; set; }
    public IButtonStateColors PlaceholderOutlineColors { get; set; }
    
    public IButtonStateColors TextColors { get; set; }
    public IButtonStateColors TextOutlineColors { get; set; }
    
    public IButtonStateColors FrameColors { get; set; }
    public ColorWrapper? ActiveFrameColor { get; set; }
    
    public Length? FrameThickness { get; set; }
    public Length? ActiveFrameThickness { get; set; }
    public string UniqueId { get; set; }
    public TextScaling Scaling { get; set; } = TextScaling.Default;
    public SharedBool Enabled { get; set; }
    public TextUpdateMode? UpdateMode { get; set; }
}