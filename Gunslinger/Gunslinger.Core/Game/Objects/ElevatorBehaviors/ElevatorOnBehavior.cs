using System;
using System.Numerics;
using Ssit.CrossX.Games.Logic;

namespace Gunslinger.Core.Game.Objects.ElevatorBehaviors;

public class ElevatorOnBehavior(Elevator elevator): Behavior
{
    private Vector2 _lastMoveDirection;
    private Vector2 _lastTargetPoint;

    protected override void OnEnterState()
    {
        _lastMoveDirection = Vector2.Zero;
        _lastTargetPoint = new Vector2(-10000);
    }

    protected override bool OnFixedUpdate(float dt)
    {
        if (!elevator.IsOn)
        {
            elevator.SetState("Off");
            return true;
        }
        
        while (true)
        {
            var diff = elevator.CurrentTarget.Position - elevator.Body.Position;

            var moveDir = diff;
            if (diff.Length() > 0)
            {
                moveDir = Vector2.Normalize(diff);
            }

            if (_lastMoveDirection.Length() != 0 && (Vector2.Dot(moveDir, _lastMoveDirection) < 0 || moveDir.Length() < 0.01f))
            {
                _lastTargetPoint = elevator.CurrentTarget.Position;
                var nextTarget = elevator.CurrentTarget.Next ?? elevator;
                
                elevator.CurrentTarget = nextTarget;
                _lastMoveDirection = Vector2.Zero;
                continue;
            }

            var speed = elevator.Speed;

            if (elevator.BrakingDistance > 0)
            {
                var factor = diff.Length() / elevator.BrakingDistance + 0.2f;
                var factor2 = (_lastTargetPoint - elevator.Body.Position).Length() / elevator.BrakingDistance + 0.2f;
                
                factor = MathF.Min(MathF.Min(factor2, factor), 1);
                factor = MathF.Sin(factor * MathF.PI / 2);
                speed *= factor;
                
                speed = MathF.Max(speed, 0.01f);
            }
            
            speed = MathF.Min(diff.Length() / dt, speed);
            elevator.Body.LinearVelocity = speed * moveDir;
            
            _lastMoveDirection = moveDir;
            break;
        }

        return true;
    }
}