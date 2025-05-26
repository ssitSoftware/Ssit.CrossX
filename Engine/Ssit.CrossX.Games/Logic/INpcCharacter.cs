using System.Numerics;
using System.Threading.Tasks;
using Ssit.CrossX.Games.Physics.Dynamics;

namespace Ssit.CrossX.Games.Logic;

public interface INpcCharacter
{
    bool CanStartConversation { get; }
    void PrepareCameraForTalking();
    Task StartConversation(float posX);
    float? TalkingDistance { get; }
    Body Body { get; }
}