using System;
using System.Numerics;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Input;

namespace Gunslinger.Core.Game.Objects.PlayerBehaviors;

public class JumpBehavior(Player player, IInputMappings inputMappings) : Behavior
{
    protected override bool OnUpdate(float dt)
    {
        if (!player.IsOnGround)
        {
            return false;
        }
        
        if (inputMappings[0].GetButton(GameControls.Jump) == ButtonState.JustPressed && inputMappings[0].GetAxis(GameControls.Vertical) < 0.75f)
        {
            player.SetState("Jump");
            
            var currentY = MathF.Min(player.Body.LinearVelocity.Y, 0);
            player.Body.LinearVelocity = player.Body.LinearVelocity with {Y = currentY - GamePhysics.JumpVelocity} + (player.MomentumOffset / dt) with { Y = 0 };
            player.Body.Position -= new Vector2(0, 0.11f); 
            player.MomentumOffset = Vector2.Zero;
            return true;
        }
        
        return false;
    }
}