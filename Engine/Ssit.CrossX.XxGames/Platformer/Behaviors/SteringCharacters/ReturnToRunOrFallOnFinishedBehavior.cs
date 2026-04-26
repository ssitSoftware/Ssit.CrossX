using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Stering;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteringCharacters;

public class ReturnToRunOrFallOnFinishedBehavior : SteringBehavior<ISteringCharacter>
{
    protected override bool OnSequenceFinished(ISteringCharacter obj, string name)
    {
        obj.SetSteringState(obj.SteringParameters.IsOnGround ? "Run" : "Fall");
        return true;
    }
}
