using System.Diagnostics.CodeAnalysis;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Input;

namespace Nokemono.Core.Game.Objects.PlayerBehaviors;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class JumpOfPlatformBehavior(Player player, IInputMappings inputMappings) : Behavior
{
    private bool _jumpOfRequested = false;
    
    protected override bool OnFixedUpdate(float dt)
    {
        if (!player.IsOnPlatform)
            return false;
        
        if (_jumpOfRequested)
        {
            _jumpOfRequested = false;
            player.Body.Position = player.Body.Position with {Y = player.Body.Position.Y + 0.75f};
            player.Body.LinearVelocity = player.Body.LinearVelocity with {Y = 1};
            player.SetState("Jump->Fall");
            return true;
        }
        
        return false;
    }
    
    protected override bool OnUpdate(float dt)
    {
        _jumpOfRequested |= inputMappings[player.PlayerIndex].GetButton(GameControls.Jump) == ButtonState.JustPressed;
        _jumpOfRequested &= inputMappings[player.PlayerIndex].GetAxis(GameControls.Vertical) >= 0.75f;
        
        return false;
    }
}