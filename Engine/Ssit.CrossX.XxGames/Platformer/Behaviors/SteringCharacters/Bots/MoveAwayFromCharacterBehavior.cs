using System;
using System.Numerics;
using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Stering;
using Ssit.CrossX.XxGames.Physics;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteringCharacters.Bots;

public class MoveAwayFromCharacterBehavior(string afterState) : SteringBehavior<ISteringCharacter>
{
    private float _startX;
    private float _moveDirection;

    protected override bool OnCollision(ISteringCharacter obj, ICollider source, ICollider other, Vector2 impact)
    {
        if (other.AttachedBody.Owner is not ICharacter)
            return false;

        var otherCenterX = other.Aabb.Center.X;
        var ourCenterX = source.Aabb.Center.X;

        _moveDirection = otherCenterX < ourCenterX ? 1f : -1f;
        obj.FaceLeft = _moveDirection > 0f;

        _startX = obj.Body.Position.X;
        obj.SetSteringState("MoveAway");
        return true;
    }

    protected override bool OnFixedUpdate(ISteringCharacter obj, float dt)
    {
        if (_moveDirection == 0f)
            return false;

        if (MathF.Abs(obj.Body.Position.X - _startX) >= obj.PhysicsValues.MoveAwayDistance)
        {
            _moveDirection = 0f;
            obj.SetSteringState(afterState);
            return true;
        }

        obj.Body.Velocity = obj.Body.Velocity with { X = _moveDirection * obj.PhysicsValues.MoveAwaySpeed };
        return false;
    }
}
