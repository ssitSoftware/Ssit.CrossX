using Ssit.CrossX.Games.Physics.Dynamics;

namespace Ssit.CrossX.Games.Logic.Objects;

public interface ILogicOperator
{
    bool SetInRange(ILogicOperable operable, Fixture operatorFixture, bool inRange);
    bool SetInRange(INpcCharacter npc, Fixture operatorFixture, bool inRange);
    bool CheckInRange(Fixture fixtureB);
    void TalkToNpc(INpcCharacter npc, string conversationId = null);
}