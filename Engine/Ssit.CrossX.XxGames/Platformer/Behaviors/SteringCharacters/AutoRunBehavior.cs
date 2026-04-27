using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Stering;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteringCharacters;

public class AutoRunBehavior : SteringBehavior<ISteringCharacter>
{
    protected override void OnExit(ISteringCharacter obj)
    {
        base.OnExit(obj);
        obj.SoundContainer.StopLoop("Run");
    }

    protected override bool OnFixedUpdate(ISteringCharacter obj, float dt)
    {
        obj.SoundContainer.PlayLoop("Run");
        obj.Body.Velocity = obj.Body.Velocity with { X = obj.SteringInput.Value(SteringControlNames.HorizontalMove) * obj.PhysicsValues.RunSpeed };
        return false;
    }
}
