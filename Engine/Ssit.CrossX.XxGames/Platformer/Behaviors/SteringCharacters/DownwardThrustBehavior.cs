using Ssit.CrossX.Input;
using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Stering;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteringCharacters;

public class DownwardThrustBehavior : SteringBehavior<ISteringCharacter>
{
    protected override bool OnFixedUpdate(ISteringCharacter obj, float dt)
    {
        if (obj.SteringInput.Button(SteringControlNames.Attack) != ButtonState.JustPressed)
            return false;

        if (!obj.SteringInput.Button(SteringControlNames.Jump).IsDown)
            return false;

        obj.Body.Velocity = obj.Body.Velocity with { X = 0, Y = obj.PhysicsValues.ThrustDownVelocity };
        obj.SetSteringState("Thrust");
        return true;
    }
}
