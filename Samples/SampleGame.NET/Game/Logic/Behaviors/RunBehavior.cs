using System.Numerics;
using Ssit.CrossX.Games.Logic;

namespace SampleGame.Game.Logic.Behaviors;

public class RunBehavior(ShooterPlayerBrain brain) : Behavior
{
    protected override bool OnUpdate(float dt)
    {
        var moveDirection = brain.Controller.GetMoveDirection();

        brain.MoveDirection = moveDirection * 50;
        brain.AimDirection = Vector2.Zero;

        if (moveDirection.Length() > 0.05f)
        {
            brain.SetState("Run");
        }
        else
        {
            brain.SetState("Idle");
        }
        
        return false;
    }
}