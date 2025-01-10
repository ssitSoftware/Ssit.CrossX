using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Input;

namespace SampleGame.Game.Logic.Behaviors.ShooterPlayer;

public class SwordAttackBehavior(ShooterPlayerBrain brain) : Behavior
{
    protected override bool OnUpdate(float dt)
    {
        if (brain.Controller.MeleeButton == ButtonState.JustPressed)
        {
            brain.SetState("Chop");
            brain.WeaponHandler.Attack();
            return true;
        }
        return false;
    }
}