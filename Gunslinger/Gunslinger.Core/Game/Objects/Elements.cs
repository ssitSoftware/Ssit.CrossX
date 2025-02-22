using System.Numerics;
using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.Games.Logic.Objects;
using Ssit.CrossX.Games.Physics.Collision;
using Ssit.CrossX.Games.Physics.Collision.Shapes;
using Ssit.CrossX.Games.Physics.Common;
using Ssit.CrossX.Games.Physics.Extensions;

namespace Gunslinger.Core.Game.Objects;

public class CrateImpl : Pushable, IMomentumReceiver
{
    public float OffsetFactor => 0.75f;
    
    public CrateImpl(GameObjectsServices services, ObjectCreationParameters parameters) 
        : base(services, parameters)
    {
        InitializeSprite("assets:/Game/Objects/Crate");
        
        InitializePhysics(new PolygonShape(
            new Vertices([
                new Vector2(-0.625f, -0.625f),
                new Vector2(0.625f, -0.625f),
                new Vector2(0.625f, 0.625f),
                new Vector2(-0.625f, 0.625f),
            ]), 1));

        Body.Mass = 300;
        Body.LinearDamping = 5;
    }
}