using System;
using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Stering;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteeringCharacters;

public class MoveHorizontalBehavior : SteeringBehavior<ISteeringCharacter>
{
    protected override bool OnFixedUpdate(ISteeringCharacter obj, float dt)
    {
        if (!obj.SteeringParameters.IsOnGround)
            return false;

        var move = obj.SteeringInput.Value(SteeringControlNames.HorizontalMove);

        if (MathF.Abs(move) > 0.1f)
        {
            var physicsValues = obj.PhysicsValues;
            var velocity = obj.Body.Velocity;
            var factor = MathF.Max(0.01f, MathF.Min(1, obj.SteeringParameters.GroundMaterial.Friction * obj.Body.Colliders[0].Material.Friction));
            
            var newX = Math.Clamp(velocity.X + move * physicsValues.Acceleration * dt * factor, -physicsValues.RunSpeed, physicsValues.RunSpeed);

            obj.Body.Velocity = velocity with { X = newX };
            obj.FaceLeft = move < 0;
            obj.SetSteeringState(MathF.Abs(newX) < physicsValues.WalkSpeed ? "Walk" : "Run");
        }
        else
        {
            obj.SetSteeringState("Idle");
        }
        return false;
    }
}