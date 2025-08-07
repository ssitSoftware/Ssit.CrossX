using System.Numerics;
using Ssit.CrossX;
using Ssit.CrossX.Games.Audio;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.Games.Logic.Objects;
using Ssit.CrossX.Games.Physics.Collision.Shapes;
using Ssit.CrossX.Games.Physics.Dynamics;

namespace Nokemono.Core.Game.Objects;

public class Barrel: SpriteGameObject, IHittable
{
    public class Parameters
    {
        
    }
    
    private readonly ContextSoundContainer _soundContainer;
    
    private bool _isBroken;
    public Barrel(GameObjectsServices services, ObjectCreationParameters<Parameters> parameters) : base(services, parameters)
    {
        BoundsRect = new RectangleF(-2, -2, 4, 4);
        InitializeSprite("assets:/Game/Objects/Barrel");

        _soundContainer = services.Container.IoCConstruct<ContextSoundContainer>(new ContextSoundContainer.Parameters
        {
            Emitter = null 
        });
        _soundContainer.RegisterSound("Break", GamePhysics.Materials.Any, "assets:/Game/Sounds/Effects/WoodBreak.wav");
        
        Body.CreateFixture(new CircleShape(0.3f, 1)
        {
            Position = new Vector2(0, -0.6f)
        });
        
        Body.BodyType = BodyType.Static;
        Body.IsSensor = true;
        
        AddState("Idle", new State());
        AddState("Breaking", new State());
        AddState("Broken", new State());
        
        SetState("Idle");
    }

    public void Hit(Vector2 dir, float power)
    {
        if (_isBroken)
            return;
        
        SetState("Breaking");
        _soundContainer.Play("Break");
        _isBroken = true;
    }

    protected override void OnAnimationFinished(string sequenceName)
    {
        base.OnAnimationFinished(sequenceName);

        if (sequenceName == "Breaking")
        {
            SetState("Broken");
        }
    }
    
    protected override void OnDispose(bool disposing)
    {
        base.OnDispose(disposing);
        _soundContainer.Dispose();
    }
}