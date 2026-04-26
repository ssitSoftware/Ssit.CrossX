using Ssit.CrossX.Input;
using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Stering;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteringCharacters;

public class SlashBehavior : SteringBehavior<ISteringCharacter>
{
    protected override bool OnFixedUpdate(ISteringCharacter obj, float dt)
    {
        if (obj.SteringInput.Attack != ButtonState.JustPressed)
            return false;

        obj.Body.Velocity = obj.Body.Velocity
            with { X = obj.FaceLeft ? 
                -obj.PhysicsValues.SlashVelocity : obj.PhysicsValues.SlashVelocity };
        
        obj.SetSteringState("Slash");
        return true;
    }
}
