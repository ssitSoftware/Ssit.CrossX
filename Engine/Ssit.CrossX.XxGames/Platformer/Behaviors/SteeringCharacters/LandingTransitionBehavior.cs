using Ssit.CrossX.XxGames.Logic.Objects.Characters;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteeringCharacters;

public class LandingTransitionBehavior(string onGroundState) : CheckAdditionalGroundBehaviorBase
{
    protected override bool OnFixedUpdate(ISteeringCharacter obj, float dt)
    {
        if (!IsOnGroundExtra(obj))
            return false;

        obj.SetSteeringState(onGroundState);
        return true;
    }
}
