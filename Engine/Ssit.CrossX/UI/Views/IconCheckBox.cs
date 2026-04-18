using System;
using System.Windows.Input;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;

namespace Ssit.CrossX.UI.Views;

public class IconCheckBox : Background, IButtonView
{
    public string UniqueId { get; set; }
    
    public ICommand Command { get; set; }
    public object CommandParameter { get; set; }
    public TimeSpan KeyCommandDelay { get; set; } = TimeSpan.FromMilliseconds(100);
    public TimeSpan CommandDelay { get; set; } = TimeSpan.FromMilliseconds(33);
    public string CommandSoundId { get; set; }
    public ButtonCommandType EnabledCommandTypes { get; set; } = ButtonCommandType.Select;
    public IUiSounds CustomSounds { get; set; }

    public ImageSource Image { get; set; }
    public ImageSource ImagePushed { get; set; }
    public Size IconSize { get; set; }
    
    public int FrameOn { get; set; } = 1;
    public int FrameOff { get; set; } = 0;

    public SharedBool IsChecked { get; set; } = new SharedBoolValue(false);
    public SharedValue<bool> HapticFeedback { get; set; } = false;
}