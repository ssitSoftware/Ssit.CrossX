using System;
using Ssit.CrossX.Games.Logic;

namespace Gunslinger.Core.Game.Objects.PlayerBehaviors;

public class JumpToFallSequenceBehavior(Player player) : Behavior
{
    protected override bool OnSequenceFinished(string name)
    {
        if (!player.IsOnGround)
        {
            player.SetState("Fall");
            return true;
        }

        return false;
    }

    protected override bool OnFixedUpdate(float dt)
    {
        if (player.IsOnGround)
        {
            if (MathF.Abs(player.Body.LinearVelocity.X) >= GamePhysics.MinRunSpeed)
            {
                player.SetState("Run");
            }
            else
            {
                player.SetState("Idle");
            }
            return true;
        }

        return false;
    }
}