using Ssit.CrossX.Core;
using Ssit.CrossX.Utils;

namespace RetroGunslinger.Core.Game;

public interface IGameInterfaces
{
    IGameInstance Instance { get; }
    IGameDialogsUi Dialogs { get; }
}