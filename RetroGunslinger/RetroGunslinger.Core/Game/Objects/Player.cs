using System;
using System.Collections.Generic;
using System.Numerics;
using RetroGunslinger.Core.Game.Objects.PlayerBehaviors;
using Ssit.CrossX;
using Ssit.CrossX.Games.Audio;
using Ssit.CrossX.Games.Editor;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.Games.Logic.Objects;
using Ssit.CrossX.Games.Physics.Collision;
using Ssit.CrossX.Games.Physics.Collision.Shapes;
using Ssit.CrossX.Games.Physics.Dynamics;
using Ssit.CrossX.Games.Physics.Extensions;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.Graphics.Sprites;

namespace RetroGunslinger.Core.Game.Objects;

public class Player : SpriteGameObject, IMomentumReceiver, ILogicOperator
{
    public class PlayerStats
    {
        // Player statistics
        [EditorInt(0, 3)] public int Jump { get; set; }
        [EditorInt(-1, 3)] public int WallClimb { get; set; } = -1;
        [EditorInt(-1, 3)] public int Dash { get; set; } = -1;
        [EditorInt(-1, 3)] public int MagneticJump { get; set; } = -1;
    }

    public PlayerStats Stats { get; } = new();

    void IMomentumReceiver.OnMomentumPassed(Vector2 offset) => MomentumOffset = offset;

    public int PlayerIndex { get; } = 0;

    public class Parameters
    {
        [EditorFloat(10, 20)] public float Speed { get; set; }
    }

    public bool FaceLeft
    {
        get => Transform == ImageTransform.FlipHorizontal;
        set => Transform = value ? ImageTransform.FlipHorizontal : ImageTransform.None;
    }

    public ILogicOperable OperableInRange { get; private set; }
    public List<Vector2> InAirVelocity { get; } = [];
    public bool IsOnStaticGround { get; private set; }
    public bool IsOnGround { get; set; }
    public bool IsOnPlatform { get; private set; }
    public int GroundMaterial { get; private set; }

    public Vector2 MomentumOffset { get; set; }

    private readonly List<Fixture> _queryList = new();
    private readonly Fixture _detectorFixture;

    public readonly ContextSoundContainer SoundContainer;

    public Player(GameObjectsServices services, ICamera camera, ObjectCreationParameters<Parameters> parameters)
        : base(services, parameters)
    {
        InitializeSprite("assets:/Game/Objects/SwordMaster");
        
        SoundContainer = services.Container.IoCConstruct<ContextSoundContainer>(new ContextSoundContainer.Parameters
        {
            Emitter = null
        });

        SoundContainer.RegisterCharacterGroundSounds();
        
        camera.SetPrimaryTarget(Body, new Vector2(0, -2f), 6);

        Sprite.SetSequence("Idle");
        Body.IsBullet = true;
        Body.FixedRotation = true;
        Body.Friction = 1f;
        
        _detectorFixture = Body.CreateFixture(new CircleShape(0.3f, 10)
        {
            Position = new Vector2(0, -0.3f)
        });

        Body.CreateFixture(new CircleShape(0.3f, 10)
        {
            Position = new Vector2(0, -1.2f)
        });

        Body.CreateFixture(new EdgeShape(new Vector2(-0.29f, -0.3f), new Vector2(-0.29f, -1.2f)));
        Body.CreateFixture(new EdgeShape(new Vector2(0.29f, -0.3f), new Vector2(0.29f, -1.2f)));

        Body.Mass = 80;
        BoundsRect = new RectangleF(-1.5f, -4, 3, 5);
        Stats.Jump = 3;

        InitializeStates(); 
    }

    private void InitializeStates()
    {
        var idleBehavior = Services.Container.IoCConstruct<IdleBehavior>(this);
        var fallBehavior = Services.Container.IoCConstruct<FallBehavior>(this);
        var jumpBehavior = Services.Container.IoCConstruct<JumpBehavior>(this);
        var runBehavior = Services.Container.IoCConstruct<RunBehavior>(this);
        var jumpOfPlatformBehavior = Services.Container.IoCConstruct<JumpOfPlatformBehavior>(this);
        var jumpingBehavior = Services.Container.IoCConstruct<JumpingBehavior>(this);
        var jumpToFallSequenceBehavior = Services.Container.IoCConstruct<JumpToFallSequenceBehavior>(this);
        var steerInAirBehavior = Services.Container.IoCConstruct<SteerInAirBehavior>(this);
        var operateBehavior = Services.Container.IoCConstruct<OperateBehavior>(this);
        var checkLandingBehavior = Services.Container.IoCConstruct<CheckLandingBehavior>(this);

        var idleOrRunState = new State(operateBehavior, jumpOfPlatformBehavior, jumpBehavior, runBehavior, fallBehavior, idleBehavior);
        var jumpState = new State(checkLandingBehavior, jumpingBehavior, steerInAirBehavior, fallBehavior, idleBehavior);
        var jumpToFallState = new State(checkLandingBehavior, steerInAirBehavior, jumpToFallSequenceBehavior, fallBehavior, runBehavior, idleBehavior);
        var fallState = new State(checkLandingBehavior, steerInAirBehavior, fallBehavior, runBehavior, idleBehavior);

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
        Body.LinearDamping = 0;
        
        DetectOnGround();
        
        if (!IsOnGround)
        {
            MomentumOffset = Vector2.Zero;
            InAirVelocity.Add(Body.LinearVelocity);
            
            while (InAirVelocity.Count > 30)
            {
                InAirVelocity.RemoveAt(0);
            }
        }
        
        base.OnFixedUpdate(dt);
    }

    private void DetectOnGround()
    {
        IsOnGround = false;
        IsOnPlatform = true;
        IsOnStaticGround = true;
        GroundMaterial = 0;

        var leftX = FaceLeft ? 0.15f : 0.3f;
        var rightX = FaceLeft ? 0.3f : 0.15f;

        var aabb = new Aabb(Body.Position - new Vector2(leftX, 0.05f), Body.Position + new Vector2(rightX, 0.2f));
        Services.World.QueryAabbs(_queryList, ref aabb);

        foreach (var fixture in _queryList)
        {
            if (fixture.Body == Body)
                continue;

            IsOnGround = true;
            IsOnPlatform &=
                GamePhysics.GetMaterialKind(fixture.Body.MaterialIndex) == GamePhysics.MaterialKind.Platform;

            if (!fixture.Body.IsStatic)
            {
                IsOnStaticGround = false;
            } 

            GroundMaterial = Math.Max(fixture.Body.MaterialIndex, GroundMaterial);
        }

        IsOnStaticGround &= IsOnGround;
        IsOnPlatform &= IsOnGround;
        _queryList.Clear();
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

    public void SetInRange(ILogicOperable operable, Fixture fixture, bool inRange)
    {
        if (_detectorFixture != fixture)
        {
            return;
        }

        if (inRange)
        {
            OperableInRange = operable;
        }
        else if (OperableInRange == operable)
        {
            OperableInRange = null;
        }
    }

    protected override void OnSpriteEvent(SpriteInstance instance, SpriteInstance.Event @event)
    {
        SoundContainer.Play(@event.EventName, GroundMaterial);
    }

    protected override void OnDispose(bool disposing)
    {
        base.OnDispose(disposing);
        SoundContainer.Dispose();
    }
}
