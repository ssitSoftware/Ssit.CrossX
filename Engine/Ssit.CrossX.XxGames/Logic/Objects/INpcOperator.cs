using Ssit.CrossX.XxGames.Physics;

namespace Ssit.CrossX.XxGames.Logic.Objects;

public interface INpcOperator
{
    bool SetInRange(INpcCharacter npc, ICollider operatorFixture, bool inRange);
    bool TalkToNpc(INpcCharacter npc, string conversationId = null);
    INpcCharacter NpcCharacterInRange { get; }
    bool CheckInRange(ICollider fixtureB);
}