using Ssit.CrossX.Input;
using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Steering;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteeringCharacters;

public class WallJumpBehavior : SteeringBehavior<ISteeringCharacter>
{
    protected override bool OnFixedUpdate(ISteeringCharacter obj, float dt)
    {
        if (obj.SteeringInput.Button(SteeringControlNames.Jump) != ButtonState.JustPressed)
            return false;

        obj.FaceLeft = !obj.FaceLeft;
        obj.SoundContainer.Play("WallJump");
        obj.SetSteeringState("Raise");
        return true;
    }
}
