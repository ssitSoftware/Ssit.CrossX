namespace Ssit.CrossX.XxGames.Logic;

public class GameTimerSimple(float deltaTime) : IGameTimer
{
    public void Update(float dt) { }
    public float TimeDelta => deltaTime;
}