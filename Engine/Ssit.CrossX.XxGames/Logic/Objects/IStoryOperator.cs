namespace Ssit.CrossX.XxGames.Logic.Objects;

public interface IStoryOperator
{
    bool ExecuteStoryConversation(INpcCharacter npc, string conversationId = null);
}