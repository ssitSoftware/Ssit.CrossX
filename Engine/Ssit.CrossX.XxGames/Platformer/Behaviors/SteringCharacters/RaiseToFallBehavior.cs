using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Stering;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteringCharacters;

public class RaiseToFallBehavior : SteringBehavior<ISteringCharacter>
{
    protected override bool OnFixedUpdate(ISteringCharacter obj, float dt)
    {
        if (obj.Body.Velocity.Y < 0)
            return false;

        obj.SetSteringState("Fall");
        return true;
    }
}
