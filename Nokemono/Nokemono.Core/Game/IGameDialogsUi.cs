using System;
using System.Collections.Generic;
using System.Windows.Input;
using Ssit.CrossX.UI.Values;

namespace Nokemono.Core.Game;

public interface IGameDialogsUi
{
    event Action<int> FocusElement;
    ICommand ReplyCommand { get; }
    SharedBool Visible { get; }
    SharedString CurrentText { get; }
    
    IReadOnlyList<SharedBool> ReplyOptionVisible { get; }
    IReadOnlyList<SharedString> ReplyOptions { get; }
}