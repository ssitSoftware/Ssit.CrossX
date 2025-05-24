namespace Ssit.CrossX.Games.Logic;

public interface IGameState
{
    bool HasFlag(string flag);
    void SetFlag(string flag);
}