using Ssit.CrossX.Input;
using Ssit.CrossX.XxGames.Logic.Objects.Characters;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteeringCharacters;

public class JumpBehavior : CheckAdditionalGroundBehaviorBase
{
    protected override bool OnFixedUpdate(ISteeringCharacter obj, float dt)
    {
        if (!IsOnGroundExtra(obj))
            return false;
        
        if (obj.SteeringInput.Button(SteeringControlNames.Jump) != ButtonState.JustPressed)
            return false;
        
        obj.SoundContainer.Play("Jump");
        obj.SetSteeringState("Raise");
        return true;
    }
}
