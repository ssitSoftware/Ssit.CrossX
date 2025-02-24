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
            
            if (MathF.Abs(player.Body.LinearVelocity.X) < 1 && player.Body.LinearVelocity.Y >= -0.1f)
            {
                player.Body.LinearVelocity = player.Body.LinearVelocity with { Y = -0.1f };
            }
            return true;
        }

        var slowVelocityX = CalculateNoRunVelocity(player.Body.LinearVelocity.X, dt);
        player.Body.LinearVelocity = player.Body.LinearVelocity with {X = slowVelocityX};
        
        return false;
    }

    private float CalculateNoRunVelocity(float linearVelocityX, float dt)
    {
        var sign = MathF.Sign(linearVelocityX);
        var amplitude = linearVelocityX * sign;

        amplitude -= dt * GamePhysics.GroundDeceleration;
        amplitude = MathF.Max(0, amplitude);

        return amplitude * sign;
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