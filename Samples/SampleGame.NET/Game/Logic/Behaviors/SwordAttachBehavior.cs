using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Input;

namespace SampleGame.Game.Logic.Behaviors;

public class SwordAttachBehavior(ShooterPlayerBrain brain) : Behavior
{
    protected override bool OnUpdate(float dt)
    {
        if (brain.Controller.MeleeButton == ButtonState.JustPressed)
        {
            brain.SetState("Chop");
            return true;
        }
        return false;
    }
}