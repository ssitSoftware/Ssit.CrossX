using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Stering;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteringCharacters;

public class CheckAdditionalGroundBehaviorBase: SteringBehavior<ISteringCharacter>
{
    protected bool IsOnGroundExtra(ISteringCharacter obj)
    {
        if (!obj.SteringParameters.IsOnGround)
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
        }

        return true;
    }
}