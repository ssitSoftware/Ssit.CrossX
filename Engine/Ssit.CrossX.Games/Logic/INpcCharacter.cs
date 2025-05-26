using System.Numerics;
using System.Threading.Tasks;

namespace Ssit.CrossX.Games.Logic;

public interface INpcCharacter
{
    bool CanStartConversation { get; }
    Task StartConversation(float position);
    
    float TalkingDistance { get; }
    Vector2 Position { get; }
}