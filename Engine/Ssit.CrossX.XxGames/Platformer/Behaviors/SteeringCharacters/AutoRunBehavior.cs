using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Stering;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteeringCharacters;

public class AutoRunBehavior : SteeringBehavior<ISteeringCharacter>
{
    protected override void OnExit(ISteeringCharacter obj)
    {
        base.OnExit(obj);
        obj.SoundContainer.StopLoop("Run");
    }

    protected override bool OnFixedUpdate(ISteeringCharacter obj, float dt)
    {
        obj.SoundContainer.PlayLoop("Run");
        obj.Body.Velocity = obj.Body.Velocity with { X = obj.SteeringInput.Value(SteeringControlNames.HorizontalMove) * obj.PhysicsValues.RunSpeed };
        return false;
    }
}
