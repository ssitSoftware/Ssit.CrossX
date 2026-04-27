using System.Numerics;
using Ssit.CrossX.Input;
using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Stering;
using Ssit.CrossX.XxGames.Physics;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteringCharacters;

public class AirJumpBehavior : SteringBehavior<ISteringCharacter>
{
    public interface IAirJumpPad
    {
        bool Activate(out Vector2? direction);
    }

    protected override bool OnFixedUpdate(ISteringCharacter obj, float dt)
    {
        if (obj.SteringParameters.IsOnGround)
            return false;
        
        if (obj.SteringInput.Button(SteringControlNames.Jump) != ButtonState.JustPressed)
            return false;

        var charAabb = obj.Body.Colliders[0].Aabb;
        var feetProbe = new Aabb(
            charAabb.Left + 0.1f,
            charAabb.Bottom - 0.2f,
            charAabb.Right - 0.1f,
            charAabb.Bottom + 0.1f);

        var colliders = obj.Body.Simulation.GetColliders(feetProbe, obj.Body, colliderType: ColliderType.Trigger);
        foreach (var collider in colliders)
        {
            if (collider?.AttachedBody?.Owner is not IAirJumpPad jp)
                continue;
            
            if (jp.Activate(out var dir))
            {
                obj.SetSteringState("Raise");
                
                obj.Body.Velocity = obj.Body.Velocity with { Y = -obj.PhysicsValues.JumpVelocity };
                obj.Body.Velocity = obj.Body.Velocity with { X = obj.FaceLeft ? -obj.PhysicsValues.RunSpeed : obj.PhysicsValues.RunSpeed };
             
                if (dir.HasValue)
                {
                    obj.Body.Velocity = dir.Value;
                    if (dir.Value.X != 0)
                    {
                        obj.FaceLeft = dir.Value.X < 0;
                    }
                }
                
                return true;
            }
        }

        return false;
    }
}
