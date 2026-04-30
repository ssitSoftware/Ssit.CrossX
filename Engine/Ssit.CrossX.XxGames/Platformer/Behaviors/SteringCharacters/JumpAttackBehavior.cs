using Ssit.CrossX.Input;
using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Stering;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteringCharacters;

public class JumpAttackBehavior : SteringBehavior<ISteringCharacter>
{
    // ReSharper disable once ClassNeverInstantiated.Local
    private sealed class Parameters
    {
        public bool CanJumpAttack;
    }
    
    protected override void OnEnter(ISteringCharacter character)
    {
        base.OnEnter(character);

        var parameters = character.GetParameters<Parameters>(true);
        parameters.CanJumpAttack = character.SteringInput.Button(SteringControlNames.Jump).IsDown; 
    }

    protected override bool OnFixedUpdate(ISteringCharacter obj, float dt)
    {
        var parameters = obj.GetParameters<Parameters>(true);
        parameters.CanJumpAttack &= obj.SteringInput.Button(SteringControlNames.Jump).IsDown;
        
        if (!parameters.CanJumpAttack || obj.SteringInput.Button(SteringControlNames.Attack) != ButtonState.JustPressed )
            return false;

        obj.Body.Velocity = obj.Body.Velocity with { Y = -obj.PhysicsValues.JumpAttackRaiseVelocity };
        
        obj.SoundContainer.Play("JumpCombo");
        obj.SetSteringState("JumpCombo");
        return true;
    }
}
