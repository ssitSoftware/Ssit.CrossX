using System;
using System.Collections.Concurrent;
using Ssit.CrossX.Core;

namespace Ssit.CrossX.Games.Logic;

public class GameState(IActionScheduler actionScheduler) : IGameState
{
    public event Action StateUpdated;
    
    private readonly ConcurrentDictionary<string, bool> _flags = new();
    
    public bool HasFlag(string flag) => _flags.TryGetValue(flag, out _);

    public void SetFlag(string flag)
    {
        _flags.TryAdd(flag, true);
        actionScheduler.Schedule(() => StateUpdated?.Invoke());
    }
}