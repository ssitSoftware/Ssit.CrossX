using System;
using System.Windows.Input;
using Ssit.CrossX.UI.Values;

namespace Ssit.CrossX.UI.Views;

public class LabelButton: Label, IButtonView
{
    public ButtonStateColors TextColors { get; set; }
    public ButtonStateColors TextOutlineColors { get; set; }
    public ButtonStateColors BackgroundColors { get; set; }
    
    public ICommand Command { get; set; }
    public object CommandParameter { get; set; }
    
    public TimeSpan KeyCommandDelay { get; set; } = TimeSpan.FromMilliseconds(100);
    public TimeSpan CommandDelay { get; set; } = TimeSpan.FromMilliseconds(33);
    
    public string UniqueId { get; set; }
    public (string left, string right) HorizontalNavigation { get; set; }
    public (string up, string down) VerticalNavigation { get; set; }
}