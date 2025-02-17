using Ssit.CrossX.Core;
using Ssit.CrossX.UI.Values;

namespace Ssit.CrossX.UI.Views;

public class GameView: Background
{
    public IGameInstance GameInstance { get; set; }
    public SharedBool Active { get; set; }
    public SharedBool ShowDebug { get; set; }
}