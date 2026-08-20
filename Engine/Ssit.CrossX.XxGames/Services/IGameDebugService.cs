using Ssit.CrossX.UI.Values;

namespace Ssit.CrossX.XxGames.Services;

public interface IGameDebugService
{
    SharedBool ShowDebug { get; }
    void ToggleDebug();
}