using System.Numerics;
using Gunslinger.Core.Game.Objects.ElevatorBehaviors;
using Ssit.CrossX;
using Ssit.CrossX.Games.Editor;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.Games.Logic.Objects;
using Ssit.CrossX.Games.Physics.Collision;
using Ssit.CrossX.Games.Physics.Collision.Shapes;
using Ssit.CrossX.Games.Physics.Dynamics;
using Ssit.CrossX.Games.Physics.Extensions;

namespace Gunslinger.Core.Game.Objects;

public sealed class Elevator: SpriteGameObject, ITarget
{
    public class Parameters
    {
        [EditorFloat(0.25f, 20)] public float Speed { get; set; } = 5;
        [EditorFloat(0.0f,10f, 0.1f)] public float BrakingDistance { get; set; } = 3f;
        [EditorLink(typeof(ITarget))] public int Target { get; set; }
        [EditorLink(typeof(ISwitch))] public int Switch { get; set; }
    }

    Vector2 ITarget.Position => _initialPosition;
    ITarget ITarget.Next => _target;
    
    private ITarget _target;
    private ISwitch _switch;
    private readonly Vector2 _initialPosition;
    
    public bool IsOn => _switch?.IsOn ?? true;
    public ITarget CurrentTarget { get; set; }
    public float Speed { get; }
    public float BrakingDistance { get; }

    public Elevator(GameObjectsServices services, ObjectCreationParameters<Parameters> parameters) : base(services, parameters, "assets:/Game/Objects/Elevator")
    {
        Speed = parameters.Parameters.Speed;
        BrakingDistance = parameters.Parameters.BrakingDistance;
        
        BoundsRect = new RectangleF(-5, -2, 10, 4);
        
        Body.BodyType = BodyType.Kinematic; 
        
        Body.CreateFixture(new EdgeShape(new Vector2(-3, 0), new Vector2(3, 0))
        {
            Vertex0  = new Vector2(-3,0),
            Vertex3 = new Vector2(3,0)
        });

        Body.Mass = 500;
        
        PlatformExtension.Attach(Body, 0.25f);
        MovingStackExtension.Attach(Body, new Aabb(Vector2.Zero, 10, 0.2f));
        
        _initialPosition = parameters.Position;
        
        parameters.LinkMap.RequestLink<ITarget>(parameters.Parameters.Target, t => CurrentTarget = _target = t);
        parameters.LinkMap.RequestLink<ISwitch>(parameters.Parameters.Switch, s => _switch = s);
        
        AddState("Off", new State(new ElevatorOffBehavior(this)));
        AddState("On", new State(new ElevatorOnBehavior(this)));
        
        SetState("Off");
    }
}