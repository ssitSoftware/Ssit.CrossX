using System.Numerics;
using Ssit.CrossX.XxGames.Logic.Objects;
using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Steering;
using Ssit.CrossX.XxGames.Physics;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteeringCharacters;

public class HitInRangeBehavior(SizeF size, Vector2 offset, float attackPower = 1) : SteeringBehavior<ISteeringCharacter>
{
    protected override bool OnFixedUpdate(ISteeringCharacter obj, float dt)
    {
        var flippedOffset = new Vector2(obj.FaceLeft ? -offset.X : offset.X, offset.Y - size.Height);
        var center = obj.Body.Position + flippedOffset;

        var aabb = new Aabb(center, size);

        var colliders = obj.Body.Simulation.GetColliders(aabb, obj.Body, colliderType: ColliderType.Dynamic | ColliderType.Trigger);

        foreach (var collider in colliders)
        {
            if (collider.AttachedBody?.Owner is not IHittable { Alive: true } hittable)
                continue;

            hittable.Hit(new Vector2(obj.FaceLeft ? -1 : 1, 0), attackPower);
        }

        return false;
    }
}
