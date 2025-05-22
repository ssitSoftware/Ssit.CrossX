using System;
using System.Windows.Input;
using Ssit.CrossX.UI.Values;

namespace Ssit.CrossX.Utils;

public interface IGameDialogsUi
{
    event Action<int> FocusElement;
    ICommand ReplyCommand { get; }
    SharedBool Visible { get; }
    SharedString CurrentText { get; }
    
    SharedBool[] ReplyOptionVisible { get; }
    SharedString[] ReplyOptions { get; }
}