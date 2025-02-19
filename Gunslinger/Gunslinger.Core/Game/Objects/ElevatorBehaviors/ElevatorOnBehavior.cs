using System;
using System.Numerics;
using Ssit.CrossX.Games.Logic;

namespace Gunslinger.Core.Game.Objects.ElevatorBehaviors;

public class ElevatorOnBehavior(Elevator elevator): Behavior
{
    private Vector2 _lastMoveDirection;


    protected override void OnEnterState()
    {
        _lastMoveDirection = Vector2.Zero;
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

            if (_lastMoveDirection.Length() != 0 && (Vector2.Dot(moveDir, _lastMoveDirection) < 0 || moveDir.Length() < 0.05f))
            {
                var nextTarget = elevator.CurrentTarget.Next ?? elevator;
                
                elevator.CurrentTarget = nextTarget;
                _lastMoveDirection = Vector2.Zero;
            }

            var speed = MathF.Min(diff.Length() / dt, elevator.Speed); 
            elevator.Body.LinearVelocity = speed * moveDir;
            
            _lastMoveDirection = moveDir;
            break;
        }

        return true;
    }
}