using System;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Input;

namespace Gunslinger.Core.Game.Objects.PlayerBehaviors;

public class RunBehavior(Player player, IInputMappings inputMappings): Behavior
{
    protected override bool OnFixedUpdate(float dt)
    {
        if (!player.IsOnGround)
            return false;
        
        var move = inputMappings[player.PlayerIndex].GetAxis(GameControls.Horizontal);
        var amplitude = MathF.Abs(move);
        
        if (amplitude > 0.25f)
        {
            move = MathF.Sign(move);
            
            player.SetState("Run Fast");
            player.FaceLeft = move < 0;

            var newVelocityX = CalculateRunVelocity(player.Body.LinearVelocity.X, move, dt);
            player.Body.LinearVelocity = player.Body.LinearVelocity with {X = newVelocityX};
            
            if (MathF.Abs(player.Body.LinearVelocity.X) < 1 && player.IsOnStaticGround)
            {
                player.Body.LinearVelocity = player.Body.LinearVelocity with { Y = -3f };
            }
            
            return true;
        }
        
        return false;
    }
    
    private float CalculateRunVelocity(float linearVelocityX, float move, float dt)
    {
        var sign = MathF.Sign(move);
        var amplitude = linearVelocityX * sign;
        
        if (amplitude < GamePhysics.RunAccelerationSpeed)
        {
            amplitude += dt * GamePhysics.RunAcceleration;
            amplitude = MathF.Max(GamePhysics.MinRunSpeed, amplitude);
        }
        else
        {
            amplitude = MathF.Max(GamePhysics.RunSpeed, amplitude);
        }
        return sign * amplitude;
    }
}