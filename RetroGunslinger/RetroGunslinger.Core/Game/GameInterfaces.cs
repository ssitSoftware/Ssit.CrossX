using Ssit.CrossX.Core;
using Ssit.CrossX.Utils;

namespace RetroGunslinger.Core.Game;

public class GameInterfaces : IGameInterfaces 
{
    public IGameInstance Instance { get; set; }
    public IGameDialogsUi Dialogs { get; set; }
}