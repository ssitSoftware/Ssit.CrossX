using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.Games.Logic.Objects;
using Ssit.CrossX.Games.Physics.Collision.Shapes;

namespace Gunslinger.Core.Game.Objects.Pushables;

public class TireImpl : Pushable
{
    public TireImpl(GameObjectsServices services, ObjectCreationParameters parameters) 
        : base(services, parameters)
    {
        InitializeSprite("assets:/Game/Objects/Tire");
        InitializePhysics(new CircleShape(0.75f, 20));

        Body.Mass = 300;
        Body.LinearDamping = 0.5f;
    }
}