using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Input;

namespace Gunslinger.Core.Game.Objects.PlayerBehaviors;

public class JumpOfPlatformBehavior(Player player, IInputMappings inputMappings) : Behavior
{
    protected override bool OnFixedUpdate(float dt)
    {
        if (!player.IsOnPlatform)
            return false;
        
        if (inputMappings[0].GetButton(GameControls.Jump) == ButtonState.JustPressed)
        {
            if (inputMappings[0].GetAxis(GameControls.Vertical) >= 0.75f)
            {
                player.Body.Position = player.Body.Position with {Y = player.Body.Position.Y + 0.75f};
                player.Body.LinearVelocity = player.Body.LinearVelocity with {Y = 1};
                player.SetState("Fall");
                return true;
            }
        }
        
        return false;
    }
}