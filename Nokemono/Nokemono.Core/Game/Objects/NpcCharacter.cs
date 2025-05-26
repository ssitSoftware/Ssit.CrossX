using System;
using System.Numerics;
using System.Threading.Tasks;
using Ssit.CrossX;
using Ssit.CrossX.Content;
using Ssit.CrossX.Core;
using Ssit.CrossX.Games;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.Games.Logic.Narration;
using Ssit.CrossX.Games.Logic.Objects;
using Ssit.CrossX.Games.Physics.Collision.Shapes;
using Ssit.CrossX.Games.Physics.Dynamics;
using Ssit.CrossX.Games.Physics.Dynamics.Contacts;
using Ssit.CrossX.Games.Utils;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.Graphics.Sprites;

namespace Nokemono.Core.Game.Objects;

public abstract class NpcCharacter : SpriteGameObject, INpcCharacter
{
    private readonly IGameState _gameState;
    private readonly INarrationSystem _narrationSystem;
    private readonly ICamera _camera;
    private readonly IActionScheduler _actionScheduler;
    protected Vector2 EmojiOffset { get; init; } = new (0, -2);
    private readonly SpriteInstance _emojiInstance;

    public float? TalkingDistance { get; private set; }
    protected Vector2 CameraOffset { get; init; }
    
    public string NarrationId { get; protected set; }

    private bool _inPlayerRange;
    
    Body INpcCharacter.Body => Body;

    public void PrepareCameraForTalking()
    {
        _actionScheduler.Schedule(() =>
        {
            _camera.SetTemporaryTarget(Body, CameraOffset, 4, null, TimeSpan.FromDays(10));
        });
    }
    
    public async Task StartConversation(float position)
    {
        var tcs = new TaskCompletionSource();
        _actionScheduler.Schedule(() =>
        {
            _camera.SetTemporaryTarget(Body, CameraOffset, 4, null, TimeSpan.FromDays(10));

            FaceLeft = position < Body.Position.X;
            _emojiInstance.SetSequence("Talking");
            tcs.SetResult();
        });

        await tcs.Task;
        await Task.Delay(50);
        
        await _narrationSystem.StartNarration(NarrationId);

        var tcs2 = new TaskCompletionSource();
        
        _actionScheduler.Schedule(() =>
        {
            GameStateOnStateUpdated();
            _camera.RemoveTemporaryTarget();
            tcs2.SetResult();
        });
        
        await tcs2.Task;
    }
    
    public bool CanStartConversation => _narrationSystem.HasNarration(NarrationId);

    protected NpcCharacter(GameObjectsServices services, IContentManager contentManager, IGameState gameState, 
        INarrationSystem narrationSystem, ICamera camera, IActionScheduler actionScheduler, ObjectCreationParameters parameters)
        : base(services, parameters)
    {
        _gameState = gameState;
        _narrationSystem = narrationSystem;
        _camera = camera;
        _actionScheduler = actionScheduler;

        using var go = contentManager.Get<GameObject>("assets:/Game/Objects/Emojis");

        _emojiInstance = go.Resource.CreateSpriteInstance();
        _emojiInstance.SetSequence("None");
        
        _gameState.StateUpdated += GameStateOnStateUpdated;
    }

    protected void CreateTalkingArea(float size, float talkingDistance)
    {
        Body.BodyType = BodyType.Static;
        Body.CreateFixture(new CircleShape(size, 0.1f));
        Body.IsSensor = true;

        TalkingDistance = talkingDistance;
        
        Body.OnCollision += BodyOnOnCollision;
        Body.OnSeparation += BodyOnOnSeparation;
    }

    private void GameStateOnStateUpdated()
    {
        var hasRequest = _narrationSystem.HasRequest(NarrationId);

        if (_inPlayerRange && CanStartConversation)
        {
            _emojiInstance.SetSequence(hasRequest ? "ExclamationTalk" : "CanTalk");
        }
        else if (hasRequest)
        {
            _emojiInstance.SetSequence("Exclamation");
        }
        else
        {
            _emojiInstance.SetSequence("None");
        }
    }

    protected override void OnFixedUpdate(float dt)
    {
        base.OnFixedUpdate(dt);
        _emojiInstance.Advance(dt);
    }

    protected override void OnRender(IRenderer2 renderer, RgbaColor color)
    {
        base.OnRender(renderer, color);
        
        var pos = (Body.Position + EmojiOffset) * Services.GameTemplate.TileSize;
        pos = pos.TrimVectorToPixels(Services.GameTemplate.TrimToPixels);
        
        renderer.SpriteRenderer.Draw(_emojiInstance, pos, color: color);
    }

    protected override void OnDispose(bool disposing)
    {
        base.OnDispose(disposing);
        _emojiInstance.Dispose();
        _gameState.StateUpdated -= GameStateOnStateUpdated;
    }
    
    private void BodyOnOnSeparation(Fixture fixtureA, Fixture fixtureB)
    {
        if (fixtureB.Body.Owner is ILogicOperator lo)
        {
            if (lo.SetInRange(this, fixtureB, false))
            {
                _inPlayerRange = false;
            }

            GameStateOnStateUpdated();
        }
    }

    private bool BodyOnOnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
    {
        if (fixtureB.Body.Owner is ILogicOperator lo)
        {
            _inPlayerRange |= lo.SetInRange(this,  fixtureB, true);
            GameStateOnStateUpdated();
        }
        return true;
    }
}