using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Steering;
using Ssit.CrossX.XxGames.Platformer.Helpers;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteeringCharacters;

public class LandingTransitionBehavior(string onGroundState, CheckAdditionalGroundHelper additionalGroundHelper) : SteeringBehavior<ISteeringCharacter>
{
    protected override bool OnFixedUpdate(ISteeringCharacter obj, float dt)
    {
        if (!additionalGroundHelper.IsOnGroundExtra(obj))
            return false;

        obj.SetSteeringState(onGroundState);
        return true;
    }
}
