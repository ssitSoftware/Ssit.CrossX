using Ssit.CrossX.Input;
using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Stering;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteringCharacters;

public class AirAttackBehavior : SteringBehavior<ISteringCharacter>
{
    protected override bool OnFixedUpdate(ISteringCharacter obj, float dt)
    {
        if (obj.SteringInput.Button(SteringControlNames.Attack) != ButtonState.JustPressed)
            return false;

        obj.Body.Velocity = obj.Body.Velocity
            with { Y = obj.PhysicsValues.AirAttackDownVelocity };

        obj.SoundContainer.Play("Attack");
        obj.SetSteringState("AirAttack");
        return true;
    }
}
