using System;
using System.Windows.Input;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;

namespace Ssit.CrossX.UI.Views;

public interface IButtonView: IFocusableView
{
    ICommand Command { get; }
    object CommandParameter { get; }
    TimeSpan KeyCommandDelay { get; }
    TimeSpan CommandDelay { get; }
    string CommandSoundId { get; }
    bool EnableCommandType { get; }
    IUiSounds CustomSounds { get; }
    SharedValue<bool> HapticFeedback { get; }
}