using System.Numerics;
using Ssit.CrossX.Games.Editor;
using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.Games.Logic.Objects;
using Ssit.CrossX.Games.Physics.Collision;
using Ssit.CrossX.Games.Physics.Collision.Shapes;
using Ssit.CrossX.Games.Physics.Common;
using Ssit.CrossX.Games.Physics.Extensions;

namespace RetroGunslinger.Core.Game.Objects.Pushables;

public class CrateImpl : Pushable
{
    public class Parameters
    {
        [Editor] public bool MovingStackExtension { get; set; }
    }
    
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

        Body.Mass = 75;
        Body.LinearDamping = 2;
        Body.Friction = 1;

        if (parameters.Parameters.MovingStackExtension)
        {
            MomentumSourceExtension.Attach(Body, new Aabb(new Vector2(-0.55f, -0.7f), new Vector2(0.55f, -0.5f)));
        }
    }
}