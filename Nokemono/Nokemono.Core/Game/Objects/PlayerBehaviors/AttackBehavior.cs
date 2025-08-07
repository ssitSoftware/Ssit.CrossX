using System.Numerics;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Input;

namespace Nokemono.Core.Game.Objects.PlayerBehaviors;

public class AttackBehavior(Player player, IInputMappings inputMappings) : Behavior
{
    private bool _attackRequested;
    private bool _slam;

    protected override void OnEnterState()
    {
        _attackRequested = false;
        _slam = false;
    }

    protected override bool OnUpdate(float dt)
    {
        _attackRequested |= inputMappings[player.PlayerIndex].GetButton(GameControls.Melee) == ButtonState.JustPressed;
        
        _slam |= inputMappings[player.PlayerIndex].GetButton(GameControls.Melee) == ButtonState.JustPressed &&
                 inputMappings[player.PlayerIndex].GetAxis(GameControls.Vertical) > 0.75f;
        return false;
    }
    
    protected override bool OnFixedUpdate(float dt)
    {
        if (_attackRequested)
        {
            _attackRequested = false;
            
            var dir = player.FaceLeft ? -1 : 1;

            if (_slam)
            {
                dir = 0;
            }
            
            var currentX = GamePhysics.PlayerAttackShiftVelocity * dir + player.MomentumOffset.X / dt / 2;
            
            player.MomentumOffset = Vector2.Zero;
            player.Body.LinearVelocity = new Vector2(currentX, 0);
            player.SetState(_slam ? "Slam" : "Slash 1");
            
            player.SoundContainer.Play(_slam ? "Slam" : "Slash 1");
            
            _slam = false;
            return true;
        }

        return false;
    }
}