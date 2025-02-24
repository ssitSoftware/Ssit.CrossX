using System.Numerics;
using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.Games.Logic.Objects;
using Ssit.CrossX.Games.Physics.Collision.Shapes;

namespace Gunslinger.Core.Game.Objects.Devices;

public sealed class DetectorImpl : Detector
{
    public DetectorImpl(GameObjectsServices services, ObjectCreationParameters parameters) : base(services, parameters)
    {
        InitializeSprite("assets:/Game/Objects/Detector");
        InitializePhysics(new EdgeShape(new Vector2(-0.4f, -0.1f), new Vector2(0.4f, -0.1f)));
    }
}