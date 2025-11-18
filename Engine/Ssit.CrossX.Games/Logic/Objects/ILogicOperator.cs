using Ssit.CrossX.XxGames.Physics;

namespace Ssit.CrossX.Games.Logic.Objects;

public interface ILogicOperator
{
    bool SetInRange(ILogicOperable operable, ICollider operatorFixture, bool inRange);
    bool CheckInRange(ICollider fixtureB);
    ILogicOperable OperableInRange { get; }
}

public interface INpcOperator
{
    bool SetInRange(INpcCharacter npc, ICollider operatorFixture, bool inRange);
    bool TalkToNpc(INpcCharacter npc, string conversationId = null);
    INpcCharacter NpcCharacterInRange { get; }
    bool CheckInRange(ICollider fixtureB);
}