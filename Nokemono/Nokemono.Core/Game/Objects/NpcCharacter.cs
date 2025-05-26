using System.Numerics;
using System.Threading.Tasks;
using Ssit.CrossX;
using Ssit.CrossX.Content;
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
    protected Vector2 EmojiOffset { get; set; } = new(0, -2);
    private readonly SpriteInstance _emojiInstance;

    public float TalkingDistance { get; protected set; } = 0.9f;
    public string NarrationId { get; protected set; }

    private bool _inPlayerRange;
    
    Vector2 INpcCharacter.Position => Body.Position;
    
    public async Task StartConversation(float position)
    {
        FaceLeft = position < Body.Position.X;
        _emojiInstance.SetSequence("Talking");
        
        await _narrationSystem.StartNarration(NarrationId);
        GameStateOnStateUpdated();
    }
    
    public bool CanStartConversation => _narrationSystem.HasNarration(NarrationId);

    protected NpcCharacter(GameObjectsServices services, IContentManager contentManager, IGameState gameState, INarrationSystem narrationSystem, ObjectCreationParameters parameters)
        : base(services, parameters)
    {
        _gameState = gameState;
        _narrationSystem = narrationSystem;

        using var go = contentManager.Get<GameObject>("assets:/Game/Objects/Emojis");

        _emojiInstance = go.Resource.CreateSpriteInstance();
        _emojiInstance.SetSequence("None");
        
        _gameState.StateUpdated += GameStateOnStateUpdated;
        
        Body.BodyType = BodyType.Static;
        
        Body.CreateFixture(new CircleShape(0.98f, 0.1f)
        {
            Position = new Vector2(-0f, -0.98f)
        });
        
        Body.IsSensor = true;
        
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