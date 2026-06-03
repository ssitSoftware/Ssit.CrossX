using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Stering;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteeringCharacters;

public class ReturnToIdleOnFinishedBehavior : SteeringBehavior<ISteeringCharacter>
{
    protected override bool OnSequenceFinished(ISteeringCharacter obj, string name)
    {
        obj.SetSteeringState("Idle");
        return true;
    }
}