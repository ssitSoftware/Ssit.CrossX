using System.Numerics;
using Ssit.CrossX.Games.Logic;

namespace Gunslinger.Core.Game.Objects.ElevatorBehaviors;

public class ElevatorOffBehavior(Elevator elevator): Behavior
{
    protected override void OnEnterState()
    {
        elevator.Body.LinearVelocity = Vector2.Zero;
    }

    protected override bool OnFixedUpdate(float dt)
    {
        if (elevator.IsOn)
        {
            elevator.SetState("On");
            return true;
        }

        return false;
    }
}