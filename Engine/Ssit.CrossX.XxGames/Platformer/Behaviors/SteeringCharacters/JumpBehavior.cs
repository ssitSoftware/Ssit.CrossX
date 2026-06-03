using Ssit.CrossX.Input;
using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Stering;
using Ssit.CrossX.XxGames.Platformer.Helpers;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteeringCharacters;

public class JumpBehavior(CheckAdditionalGroundHelper additionalGroundHelper) : SteeringBehavior<ISteeringCharacter>
{
    protected override bool OnFixedUpdate(ISteeringCharacter obj, float dt)
    {
        if (!additionalGroundHelper.IsOnGroundExtra(obj))
            return false;
        
        if (obj.SteeringInput.Button(SteeringControlNames.Jump) != ButtonState.JustPressed)
            return false;
        
        obj.SoundContainer.Play("Jump");
        obj.SetSteeringState("Raise");
        return true;
    }
}
