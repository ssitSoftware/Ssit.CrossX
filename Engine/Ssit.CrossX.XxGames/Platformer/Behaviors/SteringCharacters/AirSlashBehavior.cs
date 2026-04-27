using Ssit.CrossX.Input;
using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Stering;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteringCharacters;

public class AirSlashBehavior : SteringBehavior<ISteringCharacter>
{
    protected override bool OnFixedUpdate(ISteringCharacter obj, float dt)
    {
        if (obj.SteringInput.Attack != ButtonState.JustPressed)
            return false;

        obj.Body.Velocity = obj.Body.Velocity
            with { X = obj.FaceLeft ?
                -obj.PhysicsValues.SlashVelocity : obj.PhysicsValues.SlashVelocity };

        obj.SoundContainer.Play("Slash");
        obj.SetSteringState("AirSlash");
        return true;
    }
}
