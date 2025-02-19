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
            var currentY = MathF.Min(player.MomentumOffset.Y / dt, 0);
            var currentX = player.Body.LinearVelocity.X + player.MomentumOffset.X / dt;
            
            var amplitude = MathF.Abs(currentX);
            var maxAmplitude = MathF.Abs(player.Body.LinearVelocity.X + 0.6f * player.MomentumOffset.X / dt);
            
            if (amplitude > maxAmplitude)
            {
                currentX = MathF.Sign(currentX) * maxAmplitude;
            }
            
            player.Body.LinearVelocity = new Vector2(currentX, currentY - GamePhysics.JumpVelocity);
            player.Body.Position -= new Vector2(0, 0.11f); 
            
            player.MomentumOffset = Vector2.Zero;
            
            player.SetState("Jump");
            player.IsOnGround = false;
            return true;
        }
        
        return false;
    }
}