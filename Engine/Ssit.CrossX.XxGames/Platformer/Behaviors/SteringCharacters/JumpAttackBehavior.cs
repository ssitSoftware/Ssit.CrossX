using Ssit.CrossX.Input;
using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Stering;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteringCharacters;

public class JumpAttackBehavior : SteringBehavior<ISteringCharacter>
{
    private bool _canJumpAttack;
    private bool _jumpAttackRequested;
    
    protected override void OnEnter(ISteringCharacter character)
    {
        base.OnEnter(character);
        _canJumpAttack = character.SteringInput.Jump.IsDown;
    }
    
    protected override bool OnUpdate(ISteringCharacter obj, float dt)
    {
        _jumpAttackRequested =  obj.SteringInput.Attack == ButtonState.JustPressed;
        return base.OnUpdate(obj, dt);
    }

    protected override bool OnFixedUpdate(ISteringCharacter obj, float dt)
    {
        _canJumpAttack &= obj.SteringInput.Jump.IsDown;
        
        if (!_canJumpAttack || !_jumpAttackRequested)
            return false;

        obj.Body.Velocity = obj.Body.Velocity with { Y = -obj.PhysicsValues.JumpAttackRaiseVelocity };
        obj.SetSteringState("JumpCombo");
        
        _jumpAttackRequested = false;
        return true;
    }
}
