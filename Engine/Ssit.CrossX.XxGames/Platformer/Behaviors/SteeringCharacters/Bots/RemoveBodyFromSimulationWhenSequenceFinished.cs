using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Steering;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteeringCharacters.Bots;

public class RemoveBodyFromSimulationWhenSequenceFinished: SteeringBehavior<ISteeringCharacter>
{
    protected override bool OnSequenceFinished(ISteeringCharacter obj, string name)
    {
        obj.Body.Simulation.RemoveBody(obj.Body);
        return true;
    }
}