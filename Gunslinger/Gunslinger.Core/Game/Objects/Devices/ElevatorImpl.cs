using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.Games.Logic.Objects;

namespace Gunslinger.Core.Game.Objects.Devices;

public sealed class ElevatorImpl : Elevator
{
    public ElevatorImpl(GameObjectsServices services, ObjectCreationParameters<Parameters> parameters) : base(services, parameters)
    {
        InitializeSprite("assets:/Game/Objects/Elevator");
        InitializePhysics(parameters, 6, GamePhysics.DefaultKineticFactors);
    }
}