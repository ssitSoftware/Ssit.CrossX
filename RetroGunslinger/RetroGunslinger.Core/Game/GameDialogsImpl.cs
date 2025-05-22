using Ssit.CrossX.Core;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Utils;

namespace RetroGunslinger.Core.Game;

public class GameDialogsImpl(IActionScheduler actionScheduler) : GameDialogs(actionScheduler), IGameDialogs
{
    public bool IsConversationActive => Visible.Value;
}