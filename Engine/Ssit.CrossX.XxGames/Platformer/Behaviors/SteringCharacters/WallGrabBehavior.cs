using System;
using System.Numerics;
using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Stering;
using Ssit.CrossX.XxGames.Physics;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteringCharacters;

public class WallGrabBehavior(int grabMaterialIndex) : SteringBehavior<ISteringCharacter>
{
    private Vector2 _targetPosition;

    protected override void OnEnter(ISteringCharacter obj)
    {
        obj.Body.IsKinematic = true;
        obj.Body.Velocity = Vector2.Zero;

        var grabAabb = FindGrabAabb(obj);
        if (grabAabb.HasValue)
        {
            var charAabb = obj.Body.Colliders[0].Aabb;
            var offsetX = obj.FaceLeft
                ? grabAabb.Value.Right - charAabb.Left
                : grabAabb.Value.Left - charAabb.Right;
            var offsetY = grabAabb.Value.Center.Y - charAabb.Center.Y;
            _targetPosition = obj.Body.Position + new Vector2(offsetX, offsetY);
        }
        else
        {
            _targetPosition = obj.Body.Position;
        }
    }

    protected override void OnExit(ISteringCharacter obj)
    {
        obj.Body.IsKinematic = false;
    }

    protected override bool OnFixedUpdate(ISteringCharacter obj, float dt)
    {
        obj.Body.Position = Vector2.Lerp(obj.Body.Position, _targetPosition, MathF.Min(1f, 12f * dt));

        if (FindGrabAabb(obj) == null)
        {
            obj.SetSteringState("Fall");
            return true;
        }

        return false;
    }

    private Aabb? FindGrabAabb(ISteringCharacter obj)
    {
        var aabb = obj.Body.Colliders[0].Aabb;
        var probe = obj.FaceLeft
            ? new Aabb(aabb.Left - 0.2f, aabb.Top, aabb.Left + 0.01f, aabb.Bottom)
            : new Aabb(aabb.Right - 0.01f, aabb.Top, aabb.Right + 0.2f, aabb.Bottom);

        var colliders = obj.Body.Simulation.GetColliders(probe, obj.Body, colliderType: ColliderType.Trigger);

        Aabb? grabAabb = null;
        foreach (var collider in colliders)
        {
            if (collider.Material.Index == grabMaterialIndex)
                grabAabb = grabAabb?.Union(collider.Aabb) ?? collider.Aabb;
        }
        return grabAabb;
    }
}
