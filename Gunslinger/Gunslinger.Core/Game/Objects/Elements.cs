using System.Numerics;
using Ssit.CrossX.Games.Editor;
using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.Games.Logic.Objects;
using Ssit.CrossX.Games.Physics.Collision;
using Ssit.CrossX.Games.Physics.Collision.Shapes;
using Ssit.CrossX.Games.Physics.Common;
using Ssit.CrossX.Games.Physics.Extensions;

namespace Gunslinger.Core.Game.Objects;

public class CrateImpl : Pushable, IMomentumReceiver
{
    public class Parameters
    {
        [Editor] public bool MovingStackExtension { get; set; }
    }
    
    public float OffsetFactor => 0.75f;
    
    public CrateImpl(GameObjectsServices services, ObjectCreationParameters<Parameters> parameters) 
        : base(services, parameters)
    {
        InitializeSprite("assets:/Game/Objects/Crate");
        
        InitializePhysics(new PolygonShape(
            new Vertices([
                new Vector2(-0.625f, -0.625f),
                new Vector2(0.625f, -0.625f),
                new Vector2(0.625f, 0.625f),
                new Vector2(-0.625f, 0.625f),
            ]), 20));

        Body.Mass = 300;
        Body.LinearDamping = 5;

        if (parameters.Parameters.MovingStackExtension)
        {
            MovingStackExtension.Attach(Body, new Aabb(new Vector2(-0.55f, -0.7f), new Vector2(0.55f, -0.5f)));
        }
    }
}

public class TireImpl : Pushable
{
    public TireImpl(GameObjectsServices services, ObjectCreationParameters parameters) 
        : base(services, parameters)
    {
        InitializeSprite("assets:/Game/Objects/Tire");
        
        InitializePhysics(new CircleShape(0.75f, 20));

        Body.Mass = 300;
        Body.LinearDamping = 2;
    }
}