using System.Collections.Generic;

namespace Ssit.CrossX.Games.Logic;

public class GameState : IGameState
{
    private readonly HashSet<string> _flags = new();
    
    public bool HasFlag(string flag) => _flags.Contains(flag);
    public void SetFlag(string flag) => _flags.Add(flag);
}