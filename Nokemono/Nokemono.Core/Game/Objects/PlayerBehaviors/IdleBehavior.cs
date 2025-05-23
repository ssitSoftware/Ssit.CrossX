using System.Diagnostics.CodeAnalysis;
using Ssit.CrossX.Games.Logic;

namespace Nokemono.Core.Game.Objects.PlayerBehaviors;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
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
                player.Body.LinearDamping = 4;
            }
        }
        
        if (player.Body.LinearVelocity.Y >= 0)
        {
            _canEnterIdle = true;
        }

        return false;
    }
}