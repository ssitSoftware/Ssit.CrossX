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

public class StoryTrigger: IDisposable
{
    private readonly INarrationSystem _narrationSystem;

    public class Parameters
    {
        [EditorLink(typeof(INpcCharacter))] public int Npc { get; set; }
        [Editor] public string ConversationId { get; set; }
        [EditorInt(1, 20)] public int Width { get; } = 1;
        [EditorInt(1, 20)] public int Height { get; } = 2;
    }

    private INpcCharacter _npc;
    private readonly string _conversationId;

    private readonly Body _body;
    private bool _inPlayerRange;
    
    public StoryTrigger(GameObjectsServices services, INarrationSystem narrationSystem, ObjectCreationParameters<Parameters> parameters)
    {
        _narrationSystem = narrationSystem;
        _conversationId = parameters.Parameters.ConversationId;
        parameters.LinkMap.RequestLink<INpcCharacter>(parameters.Parameters.Npc, t => _npc = t);

        _body = new Body(services.World);
        _body.BodyType = BodyType.Static;
        _body.SetTransform(parameters.Position, 0);

        var halfWidth = parameters.Parameters.Width / 2f;
        var height = parameters.Parameters.Height;
        
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

    private void BodyOnOnSeparation(Fixture fixtureA, Fixture fixtureB)
    {
        if (fixtureB.Body.Owner is ILogicOperator lo)
        {
            if (lo.CheckInRange(fixtureB))
            {
                _inPlayerRange = false;
            }
        }
    }

    private bool BodyOnOnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
    {
        var wasInRange = _inPlayerRange;
        if (fixtureB.Body.Owner is ILogicOperator lo)
        {
            _inPlayerRange |= lo.CheckInRange(fixtureB);

            if (_inPlayerRange && !wasInRange)
            {
                if (_narrationSystem.HasNarration(_conversationId))
                {
                    lo.TalkToNpc(_npc, _conversationId);
                }
            }
        }
        
        return true;
    }

    public void Dispose()
    {
        _body.OnCollision -= BodyOnOnCollision;
        _body.OnSeparation -= BodyOnOnSeparation;   
        _body.Dispose();
    }
}