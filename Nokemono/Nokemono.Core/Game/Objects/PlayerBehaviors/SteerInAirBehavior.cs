using System;
using System.Diagnostics.CodeAnalysis;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Input;

namespace Nokemono.Core.Game.Objects.PlayerBehaviors;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class SteerInAirBehavior(Player player, IInputMappings inputMappings): Behavior
{
    protected override bool OnFixedUpdate(float dt)
    {
        if (player.IsOnGround)
        {
            return false;
        }

        var move = inputMappings[player.PlayerIndex].GetAxis(GameControls.Horizontal);
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

        if (amplitude < GamePhysics.RunSpeed)
        {
            amplitude += dt * GamePhysics.AirSteerAcceleration;
            amplitude = MathF.Min(GamePhysics.RunSpeed, amplitude);
        }
        else
        {
            amplitude = MathF.Max(GamePhysics.RunSpeed, amplitude);
        }
        return sign * amplitude;
    }
    
    private float CalculateNoAccVelocity(float linearVelocityX, float dt)
    {
        var sign = MathF.Sign(linearVelocityX);
        var amplitude = linearVelocityX * sign;

        amplitude -= dt * GamePhysics.AirBrakeDeceleration;
        amplitude = MathF.Max(0, amplitude);

        return amplitude * sign;
    }
}