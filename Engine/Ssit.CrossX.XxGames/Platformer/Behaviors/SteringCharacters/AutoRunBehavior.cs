using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Stering;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteringCharacters;

public class AutoRunBehavior : SteringBehavior<ISteringCharacter>
{
    protected override bool OnFixedUpdate(ISteringCharacter obj, float dt)
    {
        obj.Body.Velocity = obj.Body.Velocity with { X = obj.SteringInput.HorizontalMove * obj.PhysicsValues.RunSpeed };
        return false;
    }
}
