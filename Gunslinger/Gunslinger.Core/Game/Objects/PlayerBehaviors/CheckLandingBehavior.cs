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
            player.SoundContainer.Play("Land", player.GroundMaterial);
        }

        return false;
    }
}