using Ssit.CrossX.Core;

namespace Nokemono.Core.Game;

public class GameInterfaces : IGameInterfaces 
{
    public IGameInstance Instance { get; set; }
    public IGameDialogsUi Dialogs { get; set; }
}