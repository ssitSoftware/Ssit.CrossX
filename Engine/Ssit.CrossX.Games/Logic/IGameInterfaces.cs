using Ssit.CrossX.Core;

namespace Ssit.CrossX.Games.Logic;

public interface IGameInterfaces
{
    IGameInstance Instance { get; }
    IGameDialogsUi Dialogs { get; }
}