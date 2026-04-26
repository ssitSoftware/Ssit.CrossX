using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Stering;
using Ssit.CrossX.XxGames.Physics;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteringCharacters;

public class WallSlideExitBehavior : SteringBehavior<ISteringCharacter>
{
    protected override bool OnFixedUpdate(ISteringCharacter obj, float dt)
    {
        var aabb = obj.Body.Colliders[0].Aabb;
        var wallProbe = obj.FaceLeft
            ? new Aabb(aabb.Left - 0.1f, aabb.Top + 0.1f, aabb.Left, aabb.Bottom - 0.1f)
            : new Aabb(aabb.Right, aabb.Top + 0.1f, aabb.Right + 0.1f, aabb.Bottom - 0.1f);

        if (!obj.Body.Simulation.CheckCollision(wallProbe, obj.Body, 0))
        {
            obj.SetSteringState("Fall");
            return true;
        }

        return false;
    }
}
