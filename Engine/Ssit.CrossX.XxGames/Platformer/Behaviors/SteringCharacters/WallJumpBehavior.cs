using Ssit.CrossX.Input;
using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Stering;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteringCharacters;

public class WallJumpBehavior : SteringBehavior<ISteringCharacter>
{
    protected override bool OnFixedUpdate(ISteringCharacter obj, float dt)
    {
        if (obj.SteringInput.Jump != ButtonState.JustPressed)
            return false;

        obj.FaceLeft = !obj.FaceLeft;
        obj.SetSteringState("Raise");
        return true;
    }
}
