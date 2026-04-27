using Ssit.CrossX.XxGames.Logic.Objects.Characters;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteringCharacters;

public class CheckFallBehavior : CheckAdditionalGroundBehaviorBase
{
    protected override bool OnFixedUpdate(ISteringCharacter obj, float dt)
    {
        if (IsOnGroundExtra(obj))
            return false;
        
        obj.SetSteringState("Fall");
        return true;
    }
}