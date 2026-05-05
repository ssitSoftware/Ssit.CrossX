using System.Threading.Tasks;
using Ssit.CrossX.XxGames.Physics;

namespace Ssit.CrossX.XxGames.Logic;

public interface INpcCharacter
{
    void SetInRange(IBodyOwner attachedBodyOwner, bool inRange);
    bool CanStartConversation { get; }
    void PrepareCameraForTalking();
    Task StartConversation(float posX, string conversationId = null);
    float TalkingDistance { get; }
    IBody Body { get; }
}