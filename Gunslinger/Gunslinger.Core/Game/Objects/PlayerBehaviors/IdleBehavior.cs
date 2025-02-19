using System;
using Ssit.CrossX.Games.Logic;

namespace Gunslinger.Core.Game.Objects.PlayerBehaviors;

public class IdleBehavior(Player player): Behavior
{
    protected override bool OnFixedUpdate(float dt)
    {
        if (player.IsOnGround)
        {
            if (MathF.Abs(player.Body.LinearVelocity.Y) < 1)
            {
                if (MathF.Abs(player.Body.LinearVelocity.X) < GamePhysics.MinRunSpeed)
                {
                    player.SetState("Idle");
                }
                else
                {
                    player.SetState("Run");
                }
            }
        }

        return false;
    }
}