using System.Numerics;
using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Stering;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteringCharacters;

public class JumpHoldBehavior : SteringBehavior<ISteringCharacter>
{
    protected override bool OnFixedUpdate(ISteringCharacter obj, float dt)
    {
        if (obj.Body.Velocity.Y >= 0 || !obj.SteringInput.Jump.IsDown)
            return false;

        var physicsValues = obj.PhysicsValues;
        var acc = physicsValues.JumpHoldAccelFactor * physicsValues.JumpVelocity
                  + physicsValues.JumpFactor * physicsValues.JumpHoldAccelInc * physicsValues.JumpVelocity;
        obj.Body.Velocity += new Vector2(0, -acc * dt);
        return false;
    }
}
