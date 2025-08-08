using System;
using System.Numerics;
using Nokemono.Core.Game.Helpers;
using Ssit.CrossX;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.Games.Logic.Objects;
using Ssit.CrossX.Games.Physics.Collision.Shapes;
using Ssit.CrossX.Games.Physics.Dynamics;
using Ssit.CrossX.Games.Rendering;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Font;
using Ssit.CrossX.Graphics.Renderer;

namespace Nokemono.Core.Game.Objects;

public class Dummy: SpriteGameObject, IHittable
{
    Vector2 IHittable.Position => Body.Position;
    private readonly HitValuePresenter _hitValuePresenter;
    
    private readonly int _particleSystemContext;
    
    public Dummy(GameObjectsServices services, ObjectCreationParameters parameters, IPaletteSource paletteSource, IFontsManager fontsManager) : base(services, parameters)
    {
        _particleSystemContext = services.ParticleSystem.RequestContextId();
        _hitValuePresenter = services.Container.IoCConstruct<HitValuePresenter>();
        
        BoundsRect = new RectangleF(-2, -2, 4, 4);
        InitializeSprite("assets:/Game/Objects/Dummy");

        Body.CreateFixture(new CircleShape(0.3f, 1)
        {
            Position = new Vector2(0, -0.6f)
        });
        
        Body.BodyType = BodyType.Static;
        Body.IsSensor = true;
        
        AddState("Idle", new State());
        AddState("Left", new State());
        AddState("Right", new State());
        
        SetState("Idle");
    }

    protected override void OnRender(IRenderer2 renderer, RgbaColor color)
    {
        Services.ParticleSystem.Draw(renderer, _particleSystemContext);
        base.OnRender(renderer, color);
        _hitValuePresenter.Render(renderer);
    }

    protected override void OnUpdate(float dt)
    {
        base.OnUpdate(dt);
        _hitValuePresenter.Update(dt);
    }

    public bool Hit(Vector2 dir, float power)
    {
        SetState(dir.X > 0 ? "Right" : "Left");
        
        _hitValuePresenter.AddValue((int)(power*10), Body.Position);
        
        var sqPower = MathF.Sqrt(MathF.Sqrt(power));
        
        Services.CommonSoundContainer.Play("HitDummy", MathF.Min(1, MathF.Sqrt(power) / 3f));
        
        dir.Y -= 0.7f;
        dir = Vector2.Normalize(dir);
        
        Services.ParticleSystem.SpreadParticles(_particleSystemContext, GameConstants.ShredsParticles,  (int)(power * power + 2), Body.Position - new Vector2(0, 0.5f), 
            dir, new Vector2(0, GamePhysics.GravityAcceleration / 4), 4, 6 * sqPower, 0.125f, 0.33f, MathF.PI / 6);
        
        return true;
    }

    protected override void OnAnimationFinished(string sequenceName)
    {
        base.OnAnimationFinished(sequenceName);

        if (sequenceName is "Right" or "Left")
        {
            SetState("Idle");
        }
    }
}