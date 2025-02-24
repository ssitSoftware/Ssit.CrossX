using System;
using Ssit.CrossX.Games.Logic;

namespace Gunslinger.Core.Game.Objects.PlayerBehaviors;

public class IdleBehavior(Player player): Behavior
{
    private bool _canEnterIdle;
    
    protected override void OnEnterState()
    {
        base.OnEnterState();
        _canEnterIdle = player.CurrentState != "Jump";
    }

    protected override bool OnFixedUpdate(float dt)
    {
        if (player.IsOnGround)
        {
            if (_canEnterIdle)
            {
                player.SetState("Idle");
            }
        }
        
        if (player.Body.LinearVelocity.Y >= 0)
        {
            _canEnterIdle = true;
        }

        return false;
    }
}