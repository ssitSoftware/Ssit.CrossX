using Ssit.CrossX.Games.Physics.Dynamics;

namespace Ssit.CrossX.Games.Logic.Objects;

public interface ILogicOperator
{
    bool SetInRange(ILogicOperable operable, Fixture operatorFixture, bool inRange);
    bool CheckInRange(Fixture fixtureB);
    ILogicOperable OperableInRange { get; }
}

public interface INpcOperator
{
    bool SetInRange(INpcCharacter npc, Fixture operatorFixture, bool inRange);
    bool TalkToNpc(INpcCharacter npc, string conversationId = null);
    INpcCharacter NpcCharacterInRange { get; }
    bool CheckInRange(Fixture fixtureB);
}