using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Nokemono.Core.Game.Objects.PlayerBehaviors;
using Nokemono.Core.Game.Parameters;
using Ssit.CrossX;
using Ssit.CrossX.Core;
using Ssit.CrossX.Games.Audio;
using Ssit.CrossX.Games.Editor;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.Games.Logic.Narration;
using Ssit.CrossX.Games.Logic.Objects;
using Ssit.CrossX.Games.Physics.Collision;
using Ssit.CrossX.Games.Physics.Collision.Shapes;
using Ssit.CrossX.Games.Physics.Dynamics;
using Ssit.CrossX.Games.Physics.Extensions;
using Ssit.CrossX.Games.Rendering;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.Graphics.Sprites;
using Ssit.CrossX.Input;

namespace Nokemono.Core.Game.Objects;

public class Player : SpriteGameObject, IMomentumReceiver, ILogicOperator
{
    private readonly ICamera _camera;
    private readonly IActionScheduler _actionScheduler;
    private readonly IKeyboard _keyboard;

    public class PlayerStats
    {
        // Player statistics
        public int Jump { get; set; }
        public int WallClimb { get; set; } = -1;
        public int Dash { get; set; } = -1;
        public int MagneticJump { get; set; } = -1;
    }

    public PlayerStats Stats { get; } = new();

    void IMomentumReceiver.OnMomentumPassed(Vector2 offset) => MomentumOffset = offset;

    public int PlayerIndex { get; } = 0;
    
    public class Parameters
    {
        [EditorFloat(10, 20)] public float Speed { get; set; }
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

    private readonly INarrationSystem _narrationSystem;
    private readonly IGameState _gameState;

    private float? _walkToPositionX;
    private TaskCompletionSource _walkToTaskCompletionSource;

    public INpcCharacter NpcCharacterInRange { get; set; }

    private List<IHittable> _hittableList = new();
    private readonly int _particleContext;

    public Player(GameObjectsServices services, ICamera camera, IActionScheduler actionScheduler, 
        ObjectCreationParameters<Parameters> parameters, IKeyboard keyboard, INarrationSystem narrationSystem, IGameState gameState)
        : base(services, parameters)
    {
        _particleContext = services.ParticleSystem.RequestContextId();
        _camera = camera;
        _actionScheduler = actionScheduler;
        _keyboard = keyboard;
        _narrationSystem = narrationSystem;
        _gameState = gameState;

        _gameState.StateUpdated += OnGameStateUpdated;
        
        InitializeSprite("assets:/Game/Objects/SwordMaster");
        
        SoundContainer = services.Container.IoCConstruct<ContextSoundContainer>(new ContextSoundContainer.Parameters
        {
            Emitter = null
        });

        SoundContainer.RegisterCharacterGroundSounds();
        SoundContainer.RegisterCharacterEffectSounds();
        
        camera.SetPrimaryTarget(Body, new Vector2(0, -2f), 5);
        
        Sprite.SetSequence("Idle");

        Body.SetTransform(parameters.Position - new Vector2(0, 0.25f), 0);
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
        
        Body.CollidesWith = Category.Cat1;
        Body.CollisionCategories = Category.Cat1;
        
        Body.Mass = 80;
        BoundsRect = new RectangleF(-1.5f, -4, 3, 5);
        Stats.Jump = 0;
        InitializeStates();
    }

    private void OnGameStateUpdated()
    {
        if (_narrationSystem.HasNarration("Player"))
        {
            TalkToSelf("Player");
        }
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
        var talkBehavior = Services.Container.IoCConstruct<TalkBehavior>(this);
        var attackBehavior = Services.Container.IoCConstruct<AttackBehavior>(this);
        var attackingBehavior = Services.Container.IoCConstruct<AttackingBehavior>(this);
        
        var idleOrRunState = new State(operateBehavior, attackBehavior, jumpOfPlatformBehavior, jumpBehavior, runBehavior, fallBehavior, talkBehavior, idleBehavior);
        var jumpState = new State(checkLandingBehavior, jumpingBehavior, steerInAirBehavior, fallBehavior, idleBehavior);
        var jumpToFallState = new State(checkLandingBehavior, steerInAirBehavior, jumpToFallSequenceBehavior, fallBehavior, runBehavior, idleBehavior);
        var fallState = new State(checkLandingBehavior, steerInAirBehavior, fallBehavior, runBehavior, idleBehavior);
        
        var slashState = new State(attackingBehavior);
        var emptyState = new State();

        AddState("Idle", idleOrRunState);
        AddState("Walk", idleOrRunState);
        AddState("Run Fast", idleOrRunState);

        AddState("Jump", jumpState);
        AddState("Jump->Fall", jumpToFallState);
        AddState("Fall", fallState);
        AddState("Talking", emptyState);
        AddState("WalkTo", emptyState);
        
        AddState("Slash 1", slashState);
        AddState("Slash 2", slashState);
        AddState("Slash 3", slashState);
        
        AddState("Slam", slashState);
        
        AddState("Spin Attack", slashState);

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

        if (_walkToPositionX.HasValue)
        {
            var offset = MathF.Min(GamePhysics.WalkSpeed * dt, MathF.Abs(_walkToPositionX.Value - Body.Position.X));;
            var dir = MathF.Sign(_walkToPositionX.Value - Body.Position.X);
            
            Body.LinearVelocity -= new Vector2(0, GamePhysics.GravityAcceleration * dt);
            Body.Position += new Vector2(dir * offset, 0);
            
            if (MathF.Abs(Body.Position.X - _walkToPositionX.Value) < 0.025f)
            {
                if (!_walkToTaskCompletionSource.Task.IsCompleted)
                {
                    _walkToTaskCompletionSource.SetResult();
                }

                _walkToPositionX = null;
            }
        }
    }

    private void DetectOnGround()
    {
        IsOnGround = false;
        IsOnPlatform = true;
        IsOnStaticGround = true;
        GroundMaterial = 0;

        var leftX = FaceLeft ? 0.2f : 0.3f;
        var rightX = FaceLeft ? 0.3f : 0.2f;

        var aabb = new Aabb(Body.Position - new Vector2(leftX, 0.05f), Body.Position + new Vector2(rightX, 0.2f));
        Services.World.QueryCollisionAabbs(_queryList, ref aabb, Body);

        foreach (var fixture in _queryList)
        {
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

        if (state == "Talking")
        {
            state = "Idle";
        }

        if (state == "WalkTo")
        {
            state = "Walk";
        }
        
        if (state == "Slash 3")
        {
            state = "Slash 1";
        }

        base.SetSequence(state);
        DetectOnGround();
    }

    public bool CheckInRange(Fixture fixture)
    {
        return _detectorFixture == fixture;
    }
    
    public bool SetInRange(ILogicOperable operable, Fixture fixture, bool inRange)
    {
        if (_detectorFixture != fixture)
        {
            return false;
        }

        if (inRange)
        {
            OperableInRange = operable;
            return true;
        }

        if (OperableInRange == operable)
        {
            OperableInRange = null;
            return true;
        }
        return false;
    }
    
    public bool SetInRange(INpcCharacter npcCharacter, Fixture fixture, bool inRange)
    {
        if (_detectorFixture != fixture)
        {
            return false;
        }

        if (inRange)
        {
            NpcCharacterInRange = npcCharacter;
            return true;
        }

        if (NpcCharacterInRange == npcCharacter)
        {
            NpcCharacterInRange = null;
            return true;
        }

        return false;
    }

    protected override void OnSpriteEvent(SpriteInstance instance, SpriteInstance.Event @event)
    {
        if (CurrentState == "WalkTo")
        {
            return;
        }
        
        SoundContainer.Play(@event.EventName, GroundMaterial);
        CallStateEvent(@event.EventName, 0);

        switch (@event.EventName)
        {
            case "Hit":
                var ap = @event.GetParameters<AttackParameters>();
                ProcessAttack(ap);
                break;
            
            // case "Left Foot":
            // case "Right Foot":
            //     Services.ParticleSystem.SpreadParticles(_particleContext, GameConstants.DustParticles, 4, Body.Position, 
            //         new Vector2(FaceLeft ? -1 : 1, -1), new Vector2(0, GamePhysics.GravityAcceleration / 8), 1, 2, 
            //         0.15f ,0.3f, MathF.PI / 6);
            //     break;
        }
    }

    private void ProcessAttack(AttackParameters parameters)
    {
        var gameTemplate = Services.GameTemplate;
        
        var size = new Vector2(parameters.Width, parameters.Height) / gameTemplate.TileSize;

        var aabb = FaceLeft ? 
            new Aabb(Body.Position - size, Body.Position) :
            new Aabb(Body.Position, Body.Position + size with { Y = -size.Y });
        
        Services.World.QueryAabbs(_queryList, ref aabb);
        
        _hittableList.Clear();
        
        foreach (var fixture in _queryList)
        {
            if (fixture.Body == Body)
                continue;

            if (fixture.Body.Position.X > Body.Position.X && FaceLeft ||
                fixture.Body.Position.X < Body.Position.X && !FaceLeft)
            {
                continue;
            }
                    
            if (fixture.Body.Owner is IHittable hittableObj)
            {
                _hittableList.Add(hittableObj);
            }
        }

        for (var idx = 0; idx < _hittableList.Count; idx++)
        {
            for (var idx2 = 0; idx2 < _hittableList.Count - 1; idx2++)
            {
                var h1 = _hittableList[idx2];
                var h2 = _hittableList[idx2 + 1];
                
                var d1 = MathF.Abs(h1.Position.X - Body.Position.X);
                var d2 = MathF.Abs(h2.Position.X - Body.Position.X);

                if (d2 < d1)
                {
                    _hittableList[idx2] = h2;
                    _hittableList[idx2 + 1] = h1;
                }
            }
        }

        var dir = new Vector2(FaceLeft ? -1 : 1, 0);
        var power = parameters.Value.Calculate(1);
        
        for (var idx = 0; idx < _hittableList.Count; idx++)
        {
            if (_hittableList[idx].Hit(dir, power))
            {
                break;
            }
        }
    }

    protected override void OnRender(IRenderer2 renderer, RgbaColor color)
    {
        base.OnRender(renderer, color);
        Services.ParticleSystem.Draw(renderer, _particleContext);
    }

    protected override void OnDispose(bool disposing)
    {
        _gameState.StateUpdated -= OnGameStateUpdated;
        base.OnDispose(disposing);
        SoundContainer.Dispose();
    }
    
    public async Task WalkTo(float targetPosX)
    {
        SetState("WalkTo");

        _walkToPositionX = targetPosX;
        
        var dir = MathF.Sign(targetPosX - Body.Position.X);
        FaceLeft = dir < 0;

        _walkToTaskCompletionSource = new TaskCompletionSource();
        await _walkToTaskCompletionSource.Task;
        _walkToTaskCompletionSource = null;
    }

    protected override void OnUpdate(float dt)
    {
        base.OnUpdate(dt);

        if (_keyboard.GetKey(Key.Minus) == ButtonState.JustPressed)
        {
            Stats.Jump = Math.Max(0, Stats.Jump - 1);
        }
        
        if (_keyboard.GetKey(Key.Equals) == ButtonState.JustPressed)
        {
            Stats.Jump = Math.Min(4, Stats.Jump + 1);
        }
    }

    bool ILogicOperator.TalkToNpc(INpcCharacter npc, string conversationId)
    {
        if (!IsOnGround)
            return false;
        
        if (npc is null)
        {
            TalkToSelf(conversationId);
        }
        
        TalkToNpc(npc, conversationId);
        return true;
    }

    private async void TalkToSelf(string conversationId)
    {
        Body.LinearVelocity = Vector2.Zero;
        _camera.SetTemporaryTarget(Body, new Vector2(0,-4), 4, null, TimeSpan.FromDays(10));
        
        SetState("Talking");
        
        await _narrationSystem.StartNarration(conversationId);

        var tcs2 = new TaskCompletionSource();
        _actionScheduler.Schedule(() =>
        {
            _camera.RemoveTemporaryTarget();
            tcs2.SetResult();
        });
        
        await tcs2.Task;
        await Task.Delay(200);
        
        _actionScheduler.Schedule(() =>
        { 
            SetState("Idle");
            OnGameStateUpdated();
        });
    }

    public async void TalkToNpc(INpcCharacter npc, string conversationId = null)
    {
        if (!IsOnGround)
            return;
        
        Body.LinearVelocity = Vector2.Zero;
        npc.PrepareCameraForTalking();

        var faceLeft = Body.Position.X > npc.Body.Position.X;
        
        if (conversationId is null)
        {
            SetState("WalkTo");

            var dist = npc.TalkingDistance;
            
            var targetPosX = npc.Body.Position.X + (faceLeft ? dist : -dist);
            await WalkTo(targetPosX);
        }

        _actionScheduler.Schedule(() =>
        {
            SetState("Talking");
            FaceLeft = faceLeft;
        });
        
        await npc.StartConversation(Body.Position.X, conversationId);

        var tcs = new TaskCompletionSource();
        _camera.SetTemporaryTarget(Body, new Vector2(0, -2f), 5, () =>
        {
            tcs.SetResult();
        }, TimeSpan.Zero);

        await Task.Delay(50);
        await Task.WhenAny(tcs.Task, Task.Delay(200));
        
        _actionScheduler.Schedule(() =>
        {
            SetState("Idle");
        });
    }
}
