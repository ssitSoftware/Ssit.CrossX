using Ssit.CrossX.Core;

namespace Nokemono.Core.Game;

public interface IGameInterfaces
{
    IGameInstance Instance { get; }
    IGameDialogsUi Dialogs { get; }
}