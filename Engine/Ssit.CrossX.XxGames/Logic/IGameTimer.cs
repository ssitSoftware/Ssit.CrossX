namespace Ssit.CrossX.XxGames.Logic;

public interface IGameTimer
{
    void Update(float dt);
    float TimeDelta { get; }
}