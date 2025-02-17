using Ssit.CrossX.Games.Logic;

namespace Gunslinger.Core.Game.Objects.PlayerBehaviors;

public class FallBehavior(Player player): Behavior
{
    protected override bool OnFixedUpdate(float dt)
    {
        if (player.Body.LinearVelocity.Y > GamePhysicsParameters.PlayerVelocityToFall && !player.IsOnGround)
        {
            if (player.CurrentState == "Jump")
            {
                player.SetState("Jump->Fall");
                return true;
            }
            else
            {
                player.SetState("Fall");
                return true;
            }
        }
        return false;
    }
}