using System.Numerics;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Input;

namespace SampleGame.Game.Logic.Behaviors.ShooterPlayer;

public class ShootBehavior(ShooterPlayerBrain brain) : Behavior
{
    protected override bool OnUpdate(float dt)
    {
        if (brain.WeaponHandler.IsReloading)
        {
            brain.AimDirection = Vector2.Zero;
            // TODO: Should we hide aiming when reloading?
            //return false;
        }
        
        var aimDirection = brain.Controller.GetAimDirection();
        brain.AimDirection = aimDirection;            

        if (brain.Controller.ShootButton.IsDown && aimDirection.Length() > 0.05f)
        {
            brain.WeaponHandler.Shot();
            return true;
        }

        if (brain.Controller.ReloadButton == ButtonState.JustPressed)
        {
            brain.WeaponHandler.Reload();
            return true;
        }
        
        return false;
    }
}