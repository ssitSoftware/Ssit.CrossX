using System;
using System.Numerics;
using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Stering;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteeringCharacters;

public class ApplyVerticalJumpVelocityBehavior : SteeringBehavior<ISteeringCharacter>
{
    protected override void OnEnter(ISteeringCharacter obj)
    {
        obj.SteeringParameters.JumpHorizontalVelocity = obj.Body.Velocity.X;

        if (MathF.Abs(obj.Body.Velocity.X) < 0.1f)
        {
            obj.SteeringParameters.JumpHorizontalVelocity = obj.FaceLeft ? -0.1f : 0.1f;
        }

        obj.Body.Velocity = obj.Body.Velocity with { Y = -obj.PhysicsValues.JumpVelocity };
        obj.Body.Velocity += obj.Body.KinematicVelocity with { Y = 0 };

        obj.Body.Position -= new Vector2(0, 0.22f);
        obj.SteeringParameters.IsOnGround = false;
    }
}