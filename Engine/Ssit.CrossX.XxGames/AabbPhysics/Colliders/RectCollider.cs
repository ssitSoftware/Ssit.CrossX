using System;
using System.Numerics;
using Ssit.CrossX.XxGames.AabbPhysics.Algorithms;
using Ssit.CrossX.XxGames.Physics;
using Ssit.CrossX.XxGames.Physics.Coliders;

namespace Ssit.CrossX.XxGames.AabbPhysics.Colliders;

internal class RectCollider : ICollider
{
    public IBody AttachedBody { get; }

    public ColliderType Type { get; }

    public IMaterial Material { get; }

    public Aabb Aabb => GetAabb(AttachedBody?.Position ?? Vector2.Zero);

    public string Name { get; }
    public bool IsActive { get; set; }

    public Aabb GetAabb(Vector2 position)
    {
        return new Aabb
        {
            Left = position.X + _center.X - _halfSize.Width,
            Right = position.X + _center.X + _halfSize.Width,
            Top = position.Y + _center.Y - _halfSize.Height,
            Bottom = position.Y + _center.Y + _halfSize.Height,
        };
    }

    private readonly Vector2 _center;
    private readonly SizeF _halfSize;

    public event CollisionDelegate CollisionWith;

    public void RaiseCollisionWith(bool byMyMovement, ICollider other, Vector2 impact)
    {
        CollisionWith?.Invoke(byMyMovement, other, impact);
        
        AttachedBody?.Owner?.OnCollision(this, other, impact);
        other?.AttachedBody?.Owner?.OnCollision(other, this, -impact);
    }

    public RectCollider(RectColliderCreationParameters creationParameters)
    {
        AttachedBody = creationParameters.AttachToBody;
        Type = creationParameters.Type;
        Material = creationParameters.Material;
        _center = creationParameters.Center;
        _halfSize = creationParameters.Size / 2;
        Name = creationParameters.Name;
        IsActive = creationParameters.Active;
    }

    public bool CheckCollisionWith(ICollider obstacle)
    {
        if(obstacle is RectCollider rectCollider)
        {
            return RectRectCollision.Check(this, rectCollider);
        }

        throw new NotSupportedException();
    }

    public bool GetMovementCollision(ICollider obstacle, ref Vector2 move, out Vector2 normal)
    {
        if (obstacle is RectCollider rectCollider)
        {
            return RectRectCollision.GetMovementCollision(this, rectCollider, ref move, out normal);
        }

        throw new NotSupportedException();
    }
}