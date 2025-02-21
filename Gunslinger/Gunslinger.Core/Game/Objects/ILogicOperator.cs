using Ssit.CrossX.Games.Physics.Dynamics;

namespace Gunslinger.Core.Game.Objects;

public interface ILogicOperator
{
    void SetInRange(ILogicOperable operable, Fixture operatorFixture, bool inRange);
}