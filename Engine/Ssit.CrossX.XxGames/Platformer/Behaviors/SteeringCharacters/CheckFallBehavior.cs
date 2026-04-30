using Ssit.CrossX.XxGames.Logic.Objects.Characters;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteeringCharacters;

public class CheckFallBehavior : CheckAdditionalGroundBehaviorBase
{
    protected override bool OnFixedUpdate(ISteeringCharacter obj, float dt)
    {
        if (IsOnGroundExtra(obj))
            return false;
        
        obj.SetSteeringState("Fall");
        return true;
    }
}