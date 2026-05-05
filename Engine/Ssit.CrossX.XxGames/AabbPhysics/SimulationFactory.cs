using Ssit.CrossX.XxGames.Physics;
using Ssit.CrossX.XxGames.Physics.Attributes;
using Ssit.CrossX.XxGames.Physics.Coliders;

namespace Ssit.CrossX.XxGames.AabbPhysics;

[SupportedColliders(typeof(RectColliderCreationParameters))]
public class SimulationFactory: ISimulationFactory
{
    public ISimulation CreateSimulation() => new Simulation();
}