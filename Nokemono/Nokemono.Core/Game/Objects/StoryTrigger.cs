using System;
using System.Numerics;
using Ssit.CrossX.Games.Editor;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.Games.Logic.Narration;
using Ssit.CrossX.Games.Logic.Objects;
using Ssit.CrossX.Games.Physics.Collision.Shapes;
using Ssit.CrossX.Games.Physics.Common;
using Ssit.CrossX.Games.Physics.Dynamics;
using Ssit.CrossX.Games.Physics.Dynamics.Contacts;

namespace Nokemono.Core.Game.Objects;

public class StoryTrigger: IDisposable, IUpdatable
{
    private readonly INarrationSystem _narrationSystem;

    public class Parameters
    {
        [EditorLink(typeof(INpcCharacter))] public int Npc { get; set; }
        [Editor] public string ConversationId { get; set; }
    }

    private INpcCharacter _npc;
    private readonly string _conversationId;

    private readonly Body _body;
    private ILogicOperator _operatorInRange;
    private bool _storyTriggered;
    
    public StoryTrigger(GameObjectsServices services, INarrationSystem narrationSystem, ObjectCreationParameters<Parameters> parameters)
    {
        _narrationSystem = narrationSystem;
        _conversationId = parameters.Parameters.ConversationId;
        parameters.LinkMap.RequestLink<INpcCharacter>(parameters.Parameters.Npc, t => _npc = t);

        _body = new Body(services.World);
        _body.BodyType = BodyType.Static;
        _body.SetTransform(parameters.Position, 0);
        _body.Owner = this;

        var halfWidth = 0.5f;
        var height = 4f;
        
        _body.CreateFixture(new PolygonShape(new Vertices([
            new Vector2(-halfWidth, 0),
            new Vector2(halfWidth, 0),
            new Vector2(halfWidth, -height),
            new Vector2(-halfWidth, -height)
            ]),0.1f));
        
        _body.IsSensor = true;
        
        _body.OnCollision += BodyOnOnCollision;
        _body.OnSeparation += BodyOnOnSeparation;
    }
    
    void IUpdatable.PostFixedUpdate()
    {
        if (_storyTriggered) return;

        if (!_narrationSystem.HasNarration(_conversationId)) return;
        
        _storyTriggered = _operatorInRange?.TalkToNpc(_npc, _conversationId) ?? false;
    }

    private void BodyOnOnSeparation(Fixture fixtureA, Fixture fixtureB)
    {
        if (fixtureB.Body.Owner is ILogicOperator lo)
        {
            if (lo.CheckInRange(fixtureB))
            {
                _operatorInRange = null;
                _storyTriggered = false;
            }
        }
    }

    private bool BodyOnOnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
    {
        if (fixtureB.Body.Owner is ILogicOperator lo)
        {
            if (_operatorInRange != lo)
            {
                if (lo.CheckInRange(fixtureB))
                {
                    _operatorInRange = lo;
                    _storyTriggered = false;
                }
            }
        }
        
        return true;
    }

    public void Dispose()
    {
        _body.OnCollision -= BodyOnOnCollision;
        _body.OnSeparation -= BodyOnOnSeparation;
    }
}