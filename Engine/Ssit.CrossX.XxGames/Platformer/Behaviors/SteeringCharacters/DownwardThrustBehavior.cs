using Ssit.CrossX.Input;
using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Steering;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteeringCharacters;

public class DownwardThrustBehavior : SteeringBehavior<ISteeringCharacter>
{
    protected override bool OnFixedUpdate(ISteeringCharacter obj, float dt)
    {
        if (obj.SteeringInput.Button(SteeringControlNames.Attack) != ButtonState.JustPressed)
            return false;

        if (!obj.SteeringInput.Button(SteeringControlNames.Jump).IsDown)
            return false;

        obj.Body.Velocity = obj.Body.Velocity with { X = 0, Y = obj.PhysicsValues.ThrustDownVelocity };
        obj.SetSteeringState("Thrust");
        return true;
    }
}
