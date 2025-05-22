using Ssit.CrossX.Core;

namespace RetroGunslinger.Core.Game;

public class GameInterfaces : IGameInterfaces 
{
    public IGameInstance Instance { get; set; }
    public IGameDialogs Dialogs { get; set; }
}