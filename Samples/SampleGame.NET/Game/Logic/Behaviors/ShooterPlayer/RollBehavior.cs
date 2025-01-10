using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Input;

namespace SampleGame.Game.Logic.Behaviors.ShooterPlayer;

public class RollBehavior(ShooterPlayerBrain brain) : Behavior
{
    protected override bool OnUpdate(float dt)
    {
        if (brain.Controller.RollButton == ButtonState.JustPressed)
        {
            brain.SetState("Roll");
            brain.WeaponHandler.CancelReload();
            return true;
        }
        return false;
    }
}