using System.Numerics;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Input;

namespace Gunslinger.Core.Game.Objects.PlayerBehaviors;

public class JumpingBehavior(Player player, IInputMappings inputMappings) : Behavior
{
    protected override bool OnFixedUpdate(float dt)
    {
        bool increaseJump = !(player.Body.LinearVelocity.Y >= 0 || !inputMappings[0].GetButton(GameControls.Jump).IsDown);

        if (increaseJump)
        {
            var acc = GamePhysicsParameters.JumpHoldAccel + player.Stats.Jump * GamePhysicsParameters.JumpHoldAccelInc;
            player.Body.LinearVelocity += new Vector2(0, -acc * dt);
        }
        
        return false;
    }
}