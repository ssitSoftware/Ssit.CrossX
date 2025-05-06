using System;
using System.Diagnostics.CodeAnalysis;
using Ssit.CrossX.Games.Logic;

namespace Gunslinger.Core.Game.Objects.PlayerBehaviors;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class CheckLandingBehavior(Player player) : Behavior
{
    private bool _canPlaySound;
    protected override void OnEnterState()
    {
        base.OnEnterState();
        _canPlaySound = true;
    }

    protected override bool OnFixedUpdate(float dt)
    {
        if (player.IsOnGround && _canPlaySound && player.Body.LinearVelocity.Y > -0.1f)
        {
            _canPlaySound = false;
            
            var velocity = 0f;

            foreach (var o in player.InAirVelocity)
            {
                velocity = MathF.Max(o.Y, velocity);
            }

            player.InAirVelocity.Clear();
            var power = MathF.Min(1, MathF.Max(0, velocity) / GamePhysics.GravityAcceleration);
            power = MathF.Sqrt(MathF.Pow(power, 1.5f));
            player.SoundContainer.Play("Land", player.GroundMaterial, power);
        }

        return false;
    }
}