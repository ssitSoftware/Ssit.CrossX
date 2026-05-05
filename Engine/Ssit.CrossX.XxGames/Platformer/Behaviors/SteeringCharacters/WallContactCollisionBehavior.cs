using System;
using System.Numerics;
using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Steering;
using Ssit.CrossX.XxGames.Physics;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteeringCharacters;

public class WallContactCollisionBehavior(int wallClimbMaterialIndex): SteeringBehavior<ISteeringCharacter>
{
    protected override bool OnCollision(ISteeringCharacter obj, ICollider source, ICollider other, Vector2 impact)
    {
        if (MathF.Abs(impact.X) > 0.01f)
        {
            if (other.Aabb.Bottom > source.Aabb.Center.Y)
            {
                if (other.Material.Index == wallClimbMaterialIndex)
                {
                    obj.SetSteeringState("WallClimb");
                    return true;
                }

                var stateName = obj.CurrentSteeringState.Name;
                if (!obj.SteeringParameters.IsOnGround && stateName is "Raise" or "Fall")
                {
                    obj.SetSteeringState("WallSlide");
                    return true;
                }
            }

            var aabb = source.Aabb;
            aabb.Bottom -= 0.5f;

            if (aabb.Intersects(other.Aabb) && obj.SteeringParameters.IsOnStaticGround)
            {
                obj.FaceLeft = !obj.FaceLeft;
                return true;
            }
        }

        return false;
    }
    
    
}