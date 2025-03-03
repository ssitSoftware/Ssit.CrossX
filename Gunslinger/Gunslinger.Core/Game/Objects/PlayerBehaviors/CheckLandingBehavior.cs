using System;
using System.Linq;
using Ssit.CrossX.Games.Logic;

namespace Gunslinger.Core.Game.Objects.PlayerBehaviors;

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

            var velocity = player.InAirVelocity.Max(o => o.Y);
            player.InAirVelocity.Clear();
            var power = MathF.Min(1, MathF.Max(0, velocity) / GamePhysics.GravityAcceleration);
            power = MathF.Sqrt(MathF.Pow(power, 1.5f));
            player.SoundContainer.Play("Land", player.GroundMaterial, power);
        }

        return false;
    }
}