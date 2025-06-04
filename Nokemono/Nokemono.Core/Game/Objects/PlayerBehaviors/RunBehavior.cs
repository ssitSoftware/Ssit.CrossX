using System;
using System.Diagnostics.CodeAnalysis;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Input;

namespace Nokemono.Core.Game.Objects.PlayerBehaviors;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class RunBehavior(Player player, IInputMappings inputMappings): Behavior
{
    private float _runSlow = 0;
    private int _lastDir = 0;

    protected override bool OnFixedUpdate(float dt)
    {
        if (!player.IsOnGround)
            return false;
        
        var move = inputMappings[player.PlayerIndex].GetAxis(GameControls.Horizontal);
        var amplitude = MathF.Abs(move);
        
        if (amplitude > 0.25f)
        {
            if (MathF.Sign(move) != _lastDir || inputMappings[player.PlayerIndex].GetButton(GameControls.Walk).IsDown)
            {
                _runSlow = GamePhysics.RunSlowTime;
                _lastDir = MathF.Sign(move);
            }
            
            _runSlow -= dt;
            _runSlow = MathF.Max(0, _runSlow);
            move = MathF.Sign(move);
            
            player.SetState(_runSlow > 0 ? "Walk" : "Run Fast");
            player.FaceLeft = move < 0;

            var newVelocityX = CalculateRunVelocity(player.Body.LinearVelocity.X, move, dt);
                
            player.Body.LinearVelocity = player.Body.LinearVelocity with {X = newVelocityX};
            
            if (MathF.Abs(player.Body.LinearVelocity.X) < GamePhysics.WalkSpeed && player.IsOnStaticGround)
            {
                player.Body.LinearVelocity = player.Body.LinearVelocity with { Y = -4f }; 
            }
            
            return true;
        }
        
        _runSlow += dt;
        _runSlow = MathF.Min(GamePhysics.RunSlowTime, _runSlow);
        return false;
    }
    
    private float CalculateRunVelocity(float linearVelocityX, float move, float dt)
    {
        var sign = MathF.Sign(move);
        var amplitude = linearVelocityX * sign;

        var momentumVelocity = sign * player.MomentumOffset.X / dt;
        
        if (amplitude < GamePhysics.RunAccelerationSpeed)
        {
            var factor = (GamePhysics.RunSlowTime - _runSlow) / GamePhysics.RunSlowTime;
            amplitude += dt * GamePhysics.RunAcceleration * factor;
            amplitude = MathF.Max(GamePhysics.MinRunSpeed + momentumVelocity, amplitude);
        }
        else
        {
            amplitude = MathF.Max(GamePhysics.RunSpeed, amplitude);
        }
        return sign * amplitude;
    }
}