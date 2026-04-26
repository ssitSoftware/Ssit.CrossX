using System;
using System.Numerics;
using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Stering;
using Ssit.CrossX.XxGames.Physics;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteringCharacters;

public class WallContactCollisionBehavior(int wallClimbMaterialIndex): SteringBehavior<ISteringCharacter>
{
    protected override bool OnCollision(ISteringCharacter obj, ICollider source, ICollider other, Vector2 impact)
    {
        if (MathF.Abs(impact.X) > 0.01f)
        {
            if (other.Material.Index == wallClimbMaterialIndex)
            {
                obj.SetSteringState("WallClimb");
                return true;
            }

            var stateName = obj.CurrentSteringState.Name;
            if (!obj.SteringParameters.IsOnGround && stateName is "Raise" or "Fall")
            {
                obj.SetSteringState("WallSlide");
                return true;
            }
            
            var aabb = source.Aabb;
            aabb.Bottom -= 0.5f;

            if (aabb.Intersects(other.Aabb) && obj.SteringParameters.IsOnStaticGround)
            {
                obj.FaceLeft = !obj.FaceLeft;
                return true;
            }
        }

        return false;
    }
    
    
}