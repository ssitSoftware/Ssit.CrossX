using System;

namespace Ssit.CrossX.XxGames.Logic;

public interface IGameState
{
    event Action StateUpdated;
    bool HasFlag(string flag);
    void SetFlags(string flag);
}