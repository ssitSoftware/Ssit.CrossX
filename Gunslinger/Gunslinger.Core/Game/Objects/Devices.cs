using System.Drawing;
using System.Numerics;
using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.Games.Logic.Objects;
using Ssit.CrossX.Games.Physics.Collision.Shapes;

namespace Gunslinger.Core.Game.Objects;

public sealed class SwitchImpl : Switch
{
    public SwitchImpl(GameObjectsServices services, ObjectCreationParameters<Parameters> parameters) : base(services, parameters)
    {
        InitializeSprite("assets:/Game/Objects/Switch");
        InitializePhysics(parameters, 0.5f);
    }
}

public sealed class DetectorImpl : Detector
{
    public DetectorImpl(GameObjectsServices services, ObjectCreationParameters parameters) : base(services, parameters)
    {
        InitializeSprite("assets:/Game/Objects/Detector");
        InitializePhysics(new EdgeShape(new Vector2(-0.4f, -0.1f), new Vector2(0.4f, -0.1f)));
    }
}

public sealed class ElevatorImpl : Elevator
{
    public ElevatorImpl(GameObjectsServices services, ObjectCreationParameters<Parameters> parameters) : base(services, parameters)
    {
        InitializeSprite("assets:/Game/Objects/Elevator");
        InitializePhysics(parameters, 6);
    }
}

public class MechanicalDoorImpl : MechanicalDoor
{
    public MechanicalDoorImpl(GameObjectsServices services, ObjectCreationParameters<Parameters> parameters) 
        : base(services, parameters)
    {
        InitializeSprite("assets:/Game/Objects/Door");
        InitializePhysics(parameters, new SizeF(0.5f, 3), 0.4f);
    }
}