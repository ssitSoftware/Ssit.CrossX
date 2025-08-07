using System.Numerics;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Input;

namespace Nokemono.Core.Game.Objects.PlayerBehaviors;

public class AttackingBehavior(Player player, IInputMappings inputMappings) : Behavior
{
    private bool _canAttack;
    private bool _doCombo;
    private float _dt;

    protected override void OnEnterState()
    {
        base.OnEnterState();
        _canAttack = false;
        _doCombo = false;
    }

    protected override bool OnFixedUpdate(float dt)
    {
        _dt = dt;
        var attackRequested = inputMappings[player.PlayerIndex].GetButton(GameControls.Melee) == ButtonState.JustPressed;
        attackRequested &= _canAttack;

        _doCombo |= attackRequested;
        return false;
    }

    protected override bool OnEvent(string name, float parameter)
    {
        if (name == "Combo")
        {
            _canAttack = true;
        }
        return base.OnEvent(name, parameter);
    }

    protected override bool OnSequenceFinished(string name)
    {
        _canAttack = false;
        if (_doCombo)
        {
            _doCombo = false;
            
            var dir = player.FaceLeft ? -1 : 1;
            var currentX = GamePhysics.PlayerAttackShiftVelocity * dir + player.MomentumOffset.X / _dt / 2;

            switch (name)
            {
                case "Slash 1":
                    player.MomentumOffset = Vector2.Zero;
                    player.Body.LinearVelocity = new Vector2(currentX, 0);
                    player.SetState("Slash 2");
                    player.SoundContainer.Play("Slash 2");
                    return true;
                
                case "Slash 2":
                    player.MomentumOffset = Vector2.Zero;
                    player.Body.LinearVelocity = new Vector2(currentX, 0);
                    player.SetState("Spin Attack");
                    player.SoundContainer.Play("Spin Attack");
                    return true;
            }
        }

        player.SetState("Idle");
        return false;
    }
}