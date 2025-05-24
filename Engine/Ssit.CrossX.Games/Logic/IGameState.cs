using System;

namespace Ssit.CrossX.Games.Logic;

public interface IGameState
{
    event Action StateUpdated;
    bool HasFlag(string flag);
    void SetFlag(string flag);
}