using Ssit.CrossX.Input;
using Ssit.CrossX.XxGames.Logic.Objects.Characters;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteringCharacters;

public class JumpBehavior : CheckAdditionalGroundBehaviorBase
{
    protected override bool OnFixedUpdate(ISteringCharacter obj, float dt)
    {
        if (!IsOnGroundExtra(obj))
            return false;
        
        if (obj.SteringInput.Button(SteringControlNames.Jump) != ButtonState.JustPressed)
            return false;
        
        obj.SoundContainer.Play("Jump");
        obj.SetSteringState("Raise");
        return true;
    }
}
