using System.Numerics;
using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Stering;
using Ssit.CrossX.XxGames.Physics;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteringCharacters;

public class WallClimbBehavior(int wallClimbMaterialIndex) : SteringBehavior<ISteringCharacter>()
{
    private Aabb? _climbAabb;

    protected override void OnEnter(ISteringCharacter obj)
    {
        _climbAabb = FindClimbAabb(obj);
        obj.Body.IsKinematic = true;
        obj.Body.Velocity = Vector2.Zero;

        if (_climbAabb.HasValue)
        {
            var charAabb = obj.Body.Colliders[0].Aabb;
            var offsetX = obj.FaceLeft
                ? _climbAabb.Value.Right - charAabb.Left
                : _climbAabb.Value.Left - charAabb.Right;
            obj.Body.Position += new Vector2(offsetX, 0);
        }
    }

    protected override void OnExit(ISteringCharacter obj)
    {
        obj.Body.IsKinematic = false;
        obj.SoundContainer.StopLoop("Climb");
    }

    protected override bool OnFixedUpdate(ISteringCharacter obj, float dt)
    {
        _climbAabb ??= FindClimbAabb(obj);

        if (_climbAabb == null)
        {
            obj.SetSteringState("Fall");
            return true;
        }

        obj.SoundContainer.PlayLoop("Climb");
        
        var charAabb = obj.Body.Colliders[0].Aabb;
        var climbAabb = _climbAabb.Value;
        
        // Reached the top — shift forward to stand on it
        if (charAabb.Bottom <= climbAabb.Top)
        {
            var shift = obj.FaceLeft ? -0.6f : 0.6f;
            obj.Body.Position += new Vector2(shift, 0);
            obj.Body.Velocity = Vector2.Zero;
            obj.SetSteringState("Run");
            return true;
        }

        var previousPosition = obj.Body.Position;
        obj.Body.KinematicMove(new Vector2(0, -obj.PhysicsValues.WallClimbSpeed * dt), true);
        if (obj.Body.Position.Y >= previousPosition.Y)
        {
            obj.SetSteringState("Fall");
            return true;
        }
        return false;
    }

    private Aabb? FindClimbAabb(ISteringCharacter obj)
    {
        var aabb = obj.Body.Colliders[0].Aabb;
        var probe = obj.FaceLeft
            ? new Aabb(aabb.Left - 0.2f, aabb.Top, aabb.Left + 0.01f, aabb.Bottom)
            : new Aabb(aabb.Right - 0.01f, aabb.Top, aabb.Right + 0.2f, aabb.Bottom);

        var colliders = obj.Body.Simulation.GetColliders(probe, obj.Body);

        Aabb? climbAabb = null;
        foreach (var collider in colliders)
        {
            if (collider.Material.Index == wallClimbMaterialIndex)
                climbAabb = climbAabb?.Union(collider.Aabb) ?? collider.Aabb;
        }
        return climbAabb;
    }
}
