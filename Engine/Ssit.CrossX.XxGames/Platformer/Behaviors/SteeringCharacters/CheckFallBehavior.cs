using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Stering;
using Ssit.CrossX.XxGames.Platformer.Helpers;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteeringCharacters;

public class CheckFallBehavior(CheckAdditionalGroundHelper additionalGroundHelper) : SteeringBehavior<ISteeringCharacter>
{
    protected override bool OnFixedUpdate(ISteeringCharacter obj, float dt)
    {
        if (additionalGroundHelper.IsOnGroundExtra(obj))
            return false;
        
        obj.SetSteeringState("Fall");
        return true;
    }
}