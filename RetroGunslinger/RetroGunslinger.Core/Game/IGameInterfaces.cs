using Ssit.CrossX.Core;

namespace RetroGunslinger.Core.Game;

public interface IGameInterfaces
{
    IGameInstance Instance { get; }
    IGameDialogs Dialogs { get; }
}