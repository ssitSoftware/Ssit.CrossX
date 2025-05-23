using System;
using System.Diagnostics.CodeAnalysis;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Games.Logic.Narration;
using Ssit.CrossX.Input;

namespace Nokemono.Core.Game.Objects.PlayerBehaviors;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class RunBehavior(Player player, IInputMappings inputMappings, INarrationSystem narrationSystem): Behavior
{
    private async void ShowDialog()
    {
        player.SetState("Talking");

        await narrationSystem.StartNarration("Merchant");
        
        player.SetState("Idle");
    }
    
    protected override bool OnUpdate(float dt)
    {
        base.OnUpdate(dt);
        
        if (inputMappings[0].GetButton(GameControls.Melee) == ButtonState.JustPressed)
        {
            ShowDialog();
        }

        return false;
    }

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