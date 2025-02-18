using System.Numerics;
using Gunslinger.Core.Game.Objects.PlayerBehaviors;
using Ssit.CrossX;
using Ssit.CrossX.Games.Editor;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.Games.Logic.Objects;
using Ssit.CrossX.Games.Physics.Collision;
using Ssit.CrossX.Games.Physics.Collision.Shapes;
using Ssit.CrossX.Games.Physics.Dynamics;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;

namespace Gunslinger.Core.Game.Objects;

public class Player: SpriteGameObject
{
    public class PlayerStats
    {
        // Player statistics
        [EditorInt(0, 3)] public int Jump { get; set; } = 0;
        [EditorInt(-1, 3)] public int WallClimb { get; set; } = -1;
        [EditorInt(-1, 3)] public int Dash { get; set; } = -1;
        [EditorInt(-1, 3)] public int MagneticJump { get; set; } = -1;
    }
    
    public PlayerStats Stats { get; } = new();
    
    public class Parameters
    {
        [EditorFloat(10,20)]
        public float Speed { get; set; }
    }

    public bool FaceLeft
    {
        get => Transform == ImageTransform.FlipHorizontal;
        set => Transform = value ? ImageTransform.FlipHorizontal : ImageTransform.None;
    }
    
    public bool IsOnGround { get; private set; }

    public Player(GameObjectsServices services, ICamera camera, ObjectCreationParameters<Parameters> parameters)
        : base(services, parameters, "assets:/Game/Objects/SwordMaster")
    {
        camera.SetTarget(Body, new Vector2(0, -5f));
        Sprite.SetSequence("Idle");
        Body.FixedRotation = true;
        
        Body.CreateFixture(new CircleShape(0.3f, 2)
        {
            Position = new Vector2(0,-0.3f)
        });
        
        Body.CreateFixture(new CircleShape(0.3f, 2)
        {
            Position = new Vector2(0, -1.2f)
        });
        
        Body.CreateFixture(new EdgeShape(new Vector2(-0.29f, -0.3f), new Vector2(-0.29f, -1.2f)));
        Body.CreateFixture(new EdgeShape(new Vector2(0.29f, -0.3f), new Vector2(0.29f, -1.2f)));

        BoundsRect = new RectangleF(-1.5f, -4, 3, 5);

        Stats.Jump = 2;
        
        InitializeStates();
    }

    private void InitializeStates()
    {
        var idleBehavior = Services.Container.IoCConstruct<IdleBehavior>(this);
        var fallBehavior = Services.Container.IoCConstruct<FallBehavior>(this);
        var jumpBehavior = Services.Container.IoCConstruct<JumpBehavior>(this);
        var runBehavior = Services.Container.IoCConstruct<RunBehavior>(this);
        var jumpingBehavior = Services.Container.IoCConstruct<JumpingBehavior>(this);
        var jumpToFallSequenceBehavior = Services.Container.IoCConstruct<JumpToFallSequenceBehavior>(this);
        var steerInAirBehavior = Services.Container.IoCConstruct<SteerInAirBehavior>(this);
        
        var idleOrRunState = new State(jumpBehavior, runBehavior, fallBehavior, idleBehavior);
        var jumpState = new State(jumpingBehavior, steerInAirBehavior, fallBehavior, idleBehavior);
        var jumpToFallState = new State(steerInAirBehavior, jumpToFallSequenceBehavior, idleBehavior, runBehavior);
        var fallState = new State(steerInAirBehavior, fallBehavior, idleBehavior, runBehavior);
        
        AddState("Idle", idleOrRunState);
        AddState("Run", idleOrRunState);
        AddState("Run Fast", idleOrRunState);
        
        AddState("Jump", jumpState);
        AddState("Jump->Fall", jumpToFallState);
        AddState("Fall", fallState);
        
        SetState("Idle");
    }

    protected override void OnFixedUpdate(float dt)
    {
        DetectOnGround();
        base.OnFixedUpdate(dt);
    }

    protected override void OnRender(IRenderer2 renderer, RgbaColor color)
    {
        color = color.Mix(RgbaColor.White, 0.5f);
        base.OnRender(renderer, color);
    }

    private bool SetOnGround(Fixture arg)
    {
        if (arg.Body == Body) return true;
        IsOnGround = true;
        return false;
    }
    
    private void DetectOnGround()
    {
        IsOnGround = false;
        
        var aabb = new Aabb(Body.Position - new Vector2(0.15f, 0.05f), Body.Position + new Vector2(0.15f, 0.05f));
        Services.World.QueryAABB(SetOnGround, ref aabb);
    }

    protected override void SetSequence(string state)
    {
        if (state == "Jump->Fall")
        {
            state = "Jump Transition";
        }
        base.SetSequence(state);
        DetectOnGround();
    }
}