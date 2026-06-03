using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Stering;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteeringCharacters;

public class WallSlidePhysicsBehavior : SteeringBehavior<ISteeringCharacter>
{
    protected override bool OnFixedUpdate(ISteeringCharacter obj, float dt)
    {
        obj.Body.Velocity = obj.Body.Velocity with { X = 0 };

        if (obj.Body.Velocity.Y > obj.PhysicsValues.WallSlideSpeed)
            obj.Body.Velocity = obj.Body.Velocity with { Y = obj.PhysicsValues.WallSlideSpeed };

        return false;
    }
}
