using System;
using System.Numerics;
using Ssit.CrossX.Games.Logic;

namespace SampleGame.Game.Logic.Behaviors;

public class AttackingBehavior(ShooterPlayerBrain brain) : Behavior
{
    protected override void OnEnterState()
    {
        base.OnEnterState();
        brain.MoveDirection = brain.CharacterDirection * 80;
        brain.AimDirection = Vector2.Zero;
    }

    protected override bool OnUpdate(float dt)
    {
        var dir = Vector2.Normalize(brain.MoveDirection);
        var length = brain.MoveDirection.Length();
        length -= dt * 4;
        length = MathF.Max(length, 0);

        brain.MoveDirection = dir * length;
        return true;
    }

    protected override bool OnSequenceFinished(string name)
    {
        brain.SetState("Idle");
        return true;
    }
}