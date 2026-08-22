using System.Numerics;
using Ssit.CrossX.Input;
using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Stering;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteeringCharacters;

public class WallJumpBehavior(float jumpHorizontalVelocity) : SteeringBehavior<ISteeringCharacter>
{
    protected override bool OnFixedUpdate(ISteeringCharacter obj, float dt)
    {
        if (obj.SteeringInput.Button(SteeringControlNames.Jump) != ButtonState.JustPressed)
            return false;

        obj.FaceLeft = !obj.FaceLeft;
        obj.Body.Velocity = new Vector2(obj.FaceLeft ? -jumpHorizontalVelocity : jumpHorizontalVelocity, 0);
        obj.SoundContainer?.Play("WallJump");
        obj.SetSteeringState("Raise");
        return true;
    }
}
