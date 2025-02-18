using System.Numerics;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Input;

namespace Gunslinger.Core.Game.Objects.PlayerBehaviors;

public class JumpBehavior(Player player, IInputMappings inputMappings) : Behavior
{
    protected override bool OnUpdate(float dt)
    {
        if (!player.IsOnGround)
        {
            return false;
        }
        
        if (inputMappings[0].GetButton(GameControls.Jump) == ButtonState.JustPressed)
        {
            //obj.Sounds.Play("Jump");
            player.SetState("Jump");
            player.Body.LinearVelocity = player.Body.LinearVelocity with {Y = -GamePhysicsParameters.JumpVelocity};
            player.Body.Position -= new Vector2(0, 0.11f); 
            return true;
        }
        
        return false;
    }
}