using Ssit.CrossX.Core;

namespace Ssit.CrossX.XxGames.Logic;

public interface IGameInterfaces
{
    IGameInstance Instance { get; }
    IGameDialogsUi Dialogs { get; }
}