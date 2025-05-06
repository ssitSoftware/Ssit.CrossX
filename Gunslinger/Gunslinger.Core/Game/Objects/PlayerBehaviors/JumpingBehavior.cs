using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Input;

namespace Gunslinger.Core.Game.Objects.PlayerBehaviors;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class JumpingBehavior(Player player, IInputMappings inputMappings) : Behavior
{
    protected override bool OnFixedUpdate(float dt)
    {
        bool increaseJump = !(player.Body.LinearVelocity.Y >= 0 || !inputMappings[player.PlayerIndex].GetButton(GameControls.Jump).IsDown);

        if (increaseJump)
        {
            var acc = GamePhysics.JumpHoldAccel + player.Stats.Jump * GamePhysics.JumpHoldAccelInc;
            player.Body.LinearVelocity += new Vector2(0, -acc * dt);
        }
        
        return false;
    }
}