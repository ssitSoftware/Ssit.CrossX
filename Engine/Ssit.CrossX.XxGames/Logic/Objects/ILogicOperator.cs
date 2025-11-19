using Ssit.CrossX.XxGames.Physics;

namespace Ssit.CrossX.XxGames.Logic.Objects;

public interface ILogicOperator
{
    bool SetInRange(ILogicOperable operable, ICollider operatorFixture, bool inRange);
    bool CheckInRange(ICollider fixtureB);
    ILogicOperable OperableInRange { get; }
}