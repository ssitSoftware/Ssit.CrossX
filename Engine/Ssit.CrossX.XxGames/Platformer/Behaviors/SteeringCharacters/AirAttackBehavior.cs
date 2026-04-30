using Ssit.CrossX.Input;
using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Steering;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteeringCharacters;

public class AirAttackBehavior(float horizontalMoveDivider = 1) : SteeringBehavior<ISteeringCharacter>
{
    protected override bool OnFixedUpdate(ISteeringCharacter obj, float dt)
    {
        if (obj.SteeringInput.Button(SteeringControlNames.Attack) != ButtonState.JustPressed)
            return false;

        obj.Body.Velocity = obj.Body.Velocity
            with
            {
                X = obj.Body.Velocity.X / horizontalMoveDivider,
                Y = obj.PhysicsValues.AirAttackDownVelocity
            };

        obj.SoundContainer.Play("Attack");
        obj.SetSteeringState("AirAttack");
        
        return true;
    }
}
