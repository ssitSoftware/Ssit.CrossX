using System.Threading.Tasks;
using Ssit.CrossX.XxGames.Physics;

namespace Ssit.CrossX.Games.Logic;

public interface INpcCharacter
{
    bool CanStartConversation { get; }
    void PrepareCameraForTalking();
    Task StartConversation(float posX, string conversationId = null);
    float TalkingDistance { get; }
    IBody Body { get; }
}