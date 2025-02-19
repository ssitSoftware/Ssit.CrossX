using System;
using System.Numerics;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Games.Physics.Collision;
using Ssit.CrossX.Games.Physics.Dynamics;

namespace Gunslinger.Core.Game.Objects.PlayerBehaviors;

public class FallBehavior(Player player, World world): Behavior
{
    private float _maxVelocity;
    private bool _disableFall;

    protected override void OnEnterState()
    {
        base.OnEnterState();
        _maxVelocity = 0;
    }

    protected override bool OnFixedUpdate(float dt)
    {
        _maxVelocity = MathF.Max(-player.Body.LinearVelocity.Y, _maxVelocity);
        if (player.Body.LinearVelocity.Y > 0 && !player.IsOnGround)
        {
            _disableFall = false;
            
            var aabb = new Aabb(player.Body.Position - new Vector2(0.5f, 0), player.Body.Position + new Vector2(0.5f, GamePhysics.PlayerGroundDistToFall));
            world.QueryAabb(this, Callback, ref aabb);

            if (_disableFall)
                return false;
            
            if (player.CurrentState == "Jump")
            {
                player.SetState("Jump->Fall");
                return true;
            }
            
            player.SetState("Fall");
            return true;
        }
        return false;
    }

    private bool Callback(object obj, Fixture fixture)
    {
        if (fixture.Body.Owner is Player)
            return true;
        
        var @this = (FallBehavior) obj;
        this._disableFall = true; 
        return false;
    }
}