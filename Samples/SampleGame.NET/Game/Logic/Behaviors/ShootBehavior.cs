using Ssit.CrossX.Games.Logic;

namespace SampleGame.Game.Logic.Behaviors;

public class ShootBehavior(ShooterPlayerBrain brain) : Behavior
{
    protected override bool OnUpdate(float dt)
    {
        var aimDirection = brain.Controller.GetAimDirection();
        brain.AimDirection = aimDirection;

        if (brain.Controller.ShootButton.IsDown)
        {
            brain.ShootBullet();
            return true;
        }
        
        return false;
    }
}