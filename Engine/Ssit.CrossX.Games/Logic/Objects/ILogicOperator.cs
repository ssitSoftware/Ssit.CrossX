using Ssit.CrossX.Games.Physics.Dynamics;

namespace Ssit.CrossX.Games.Logic.Objects;

public interface ILogicOperator
{
    void SetInRange(ILogicOperable operable, Fixture operatorFixture, bool inRange);
}