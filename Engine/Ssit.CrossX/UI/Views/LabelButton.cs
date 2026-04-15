using System;
using System.Windows.Input;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;

namespace Ssit.CrossX.UI.Views;

public class LabelButton: Label, IButtonView
{
    public IButtonStateColors TextColors { get; set; }
    public IButtonStateColors TextOutlineColors { get; set; }
    public IButtonStateColors BackgroundColors { get; set; }
    
    public ICommand Command { get; set; }
    public object CommandParameter { get; set; }
    public TimeSpan KeyCommandDelay { get; set; } = TimeSpan.FromMilliseconds(100);
    public TimeSpan CommandDelay { get; set; } = TimeSpan.FromMilliseconds(33);
    
    public string UniqueId { get; set; }
    public string CommandSoundId { get; set; }
    public bool EnableCommandType { get; set; }
    public IUiSounds CustomSounds { get; set; }
    public SharedValue<bool> HapticFeedback { get; set; } = false;
}