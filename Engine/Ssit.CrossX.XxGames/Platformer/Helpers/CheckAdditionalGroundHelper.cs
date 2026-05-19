using System.Collections.Generic;
using Ssit.CrossX.XxGames.Logic.Objects.Characters;

namespace Ssit.CrossX.XxGames.Platformer.Helpers;

public class CheckAdditionalGroundHelper(params int[] excludeMaterials)
{
    private readonly HashSet<int> _excludesMaterials = [..excludeMaterials];
    
    public bool IsOnGroundExtra(ISteeringCharacter obj)
    {
        if (!obj.SteeringParameters.IsOnGround)
        {
            var aabb =  obj.Body.Colliders[0].Aabb;

            if (obj.FaceLeft)
            {
                aabb.Right += 0.3f;
                aabb.Left += 0.1f;
            }
            else
            {
                aabb.Left -= 0.3f;
                aabb.Right -= 0.1f;
            }

            aabb.Top = aabb.Bottom;
            aabb.Bottom += 0.4f;
            
            var colliders = obj.Body.Simulation.GetColliders(aabb, obj.Body);
            if (colliders.Count == 0)
                return false;

            foreach (var collider in colliders)
            {
                if (_excludesMaterials.Contains(collider.Material.Index))
                {
                    return false;
                }
            }
        }

        return true;
    }
}