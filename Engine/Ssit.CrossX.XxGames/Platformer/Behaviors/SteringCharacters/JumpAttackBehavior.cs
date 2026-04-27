using Ssit.CrossX.Input;
using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Stering;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteringCharacters;

public class JumpAttackBehavior : SteringBehavior<ISteringCharacter>
{
    private bool _canJumpAttack;
    
    protected override void OnEnter(ISteringCharacter character)
    {
        base.OnEnter(character);
        _canJumpAttack = character.SteringInput.Button(SteringControlNames.Jump).IsDown;
    }

    protected override bool OnFixedUpdate(ISteringCharacter obj, float dt)
    {
        _canJumpAttack &= obj.SteringInput.Button(SteringControlNames.Jump).IsDown;
        
        if (!_canJumpAttack || obj.SteringInput.Button(SteringControlNames.Attack) != ButtonState.JustPressed )
            return false;

        obj.Body.Velocity = obj.Body.Velocity with { Y = -obj.PhysicsValues.JumpAttackRaiseVelocity };
        
        obj.SoundContainer.Play("JumpCombo");
        obj.SetSteringState("JumpCombo");
        return true;
    }
}
