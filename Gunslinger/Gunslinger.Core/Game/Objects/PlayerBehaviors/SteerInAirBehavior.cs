using System;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Input;

namespace Gunslinger.Core.Game.Objects.PlayerBehaviors;

public class SteerInAirBehavior(Player player, IInputMappings inputMappings): Behavior
{
    protected override bool OnFixedUpdate(float dt)
    {
        if (player.IsOnGround)
            return false;
        
        var move = inputMappings[0].GetAxis(GameControls.Horizontal);
        var amplitude = MathF.Abs(move);
        
        if (amplitude > 0.25f)
        {
            move = MathF.Sign(move);
            player.FaceLeft = move < 0;

            var newVelocityX = CalculateVelocity(player.Body.LinearVelocity.X, move, dt);
            player.Body.LinearVelocity = player.Body.LinearVelocity with {X = newVelocityX};
        }
        else
        {
            var slowVelocityX = CalculateNoAccVelocity(player.Body.LinearVelocity.X, dt);
            player.Body.LinearVelocity = player.Body.LinearVelocity with {X = slowVelocityX};
        }

        return false;
    }
    
    private float CalculateVelocity(float linearVelocityX, float move, float dt)
    {
        var sign = MathF.Sign(move);
        var amplitude = linearVelocityX * sign;

        if (amplitude < GamePhysicsParameters.RunSpeed)
        {
            amplitude += dt * GamePhysicsParameters.AirSteerAcceleration;
            amplitude = MathF.Min(GamePhysicsParameters.RunSpeed, amplitude);
        }
        else
        {
            amplitude = MathF.Max(GamePhysicsParameters.RunSpeed, amplitude);
        }
        return sign * amplitude;
    }
    
    private float CalculateNoAccVelocity(float linearVelocityX, float dt)
    {
        var sign = MathF.Sign(linearVelocityX);
        var amplitude = linearVelocityX * sign;

        amplitude -= dt * GamePhysicsParameters.AirBrakeDeceleration;
        amplitude = MathF.Max(0, amplitude);

        return amplitude * sign;
    }
}