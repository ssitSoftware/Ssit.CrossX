using System;
using System.Windows.Input;

namespace Ssit.CrossX.UI.Views;

public interface IButtonView: IFocusableView
{
    ICommand Command { get; }
    object CommandParameter { get; }
    TimeSpan KeyCommandDelay { get; }
    TimeSpan CommandDelay { get; }
    string CommandSoundId { get; }
}