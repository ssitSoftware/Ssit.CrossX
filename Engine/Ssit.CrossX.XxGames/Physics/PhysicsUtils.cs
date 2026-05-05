using System;

namespace Ssit.CrossX.XxGames.Physics;

public static class PhysicsUtils
{
    public static void ApplyHorizontalFriction(ICollider collider, IMaterial material, float dt)
    {
        if(collider.AttachedBody == null) return;
        
        var fr1 = collider.Material?.Friction ?? 1;
        var fr2 = material?.Friction ?? 1;
        
        var newVelocity =  collider.AttachedBody.Velocity.X * (1 - MathF.Min(1, fr1 * fr2 * dt));
        collider.AttachedBody.Velocity = collider.AttachedBody.Velocity with { X = newVelocity };
    }
}