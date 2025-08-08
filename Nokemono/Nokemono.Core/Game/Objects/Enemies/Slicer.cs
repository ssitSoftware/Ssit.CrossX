using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Nokemono.Core.Game.Helpers;
using Ssit.CrossX;
using Ssit.CrossX.Games.Editor;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.Games.Logic.Objects;
using Ssit.CrossX.Games.Physics.Collision.Shapes;
using Ssit.CrossX.Games.Physics.Dynamics;
using Ssit.CrossX.Games.Rendering;
using Ssit.CrossX.Graphics.Renderer;

namespace Nokemono.Core.Game.Objects.Enemies;

public class Slicer: SpriteGameObject, IHittable
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class Parameters
    {
        [EditorLink(typeof(ITarget))] public int Target { get; set; }
    }
    
    public Vector2 Position => Body.Position;
    
    private ITarget _target;
    private int _particleSystemContext;
    private readonly HitValuePresenter _hitValuePresenter;
    
    public Slicer(GameObjectsServices services, ObjectCreationParameters<Parameters> parameters) : base(services, parameters)
    {
        _hitValuePresenter = services.Container.IoCConstruct<HitValuePresenter>();
        _particleSystemContext = services.ParticleSystem.RequestContextId();
        
        parameters.LinkMap.RequestLink<ITarget>(parameters.Parameters.Target, t => _target = t);
        
        BoundsRect = new RectangleF(-2, -2, 4, 4);
        InitializeSprite("assets:/Game/Objects/Slicer");
        
        Body.CreateFixture(new CircleShape(0.4f, 10)
        {
            Position = new Vector2(0, -0.9f)
        });
        
        Body.CreateFixture(new CircleShape(0.4f, 10)
        {
            Position = new Vector2(0, -0.4f)
        });
        
        Body.Mass = 60;
        
        Body.SetTransform(parameters.Position - new Vector2(0, 0.25f), 0);
        
        Body.IsBullet = true;
        Body.FixedRotation = true;
        Body.Friction = 1f;

        Body.CollisionCategories = Category.Cat1;
        
        AddState("Walk", new State());
        SetState("Walk");
    }

    protected override void OnRender(IRenderer2 renderer, RgbaColor color)
    {
        base.OnRender(renderer, color);
        Services.ParticleSystem.Draw(renderer, _particleSystemContext);
        
        _hitValuePresenter.Render(renderer);
    }

    protected override void OnUpdate(float dt)
    {
        base.OnUpdate(dt);
        _hitValuePresenter.Update(dt);
    }
    
    public bool Hit(Vector2 dir, float power)
    {
        Services.CommonSoundContainer.Play("SwordFlesh");
        Body.ApplyLinearImpulse(dir * 10 * Body.Mass, Body.Position);

        dir.Y -= 0.7f;
        dir = Vector2.Normalize(dir);
        
        _hitValuePresenter.AddValue((int)(power*10), Body.Position);
        
        var sqPower = MathF.Sqrt(MathF.Sqrt(power));
        
        Services.ParticleSystem.SpreadParticles(_particleSystemContext, GameConstants.BloodParticles,  (int)(power * power + 2), Body.Position - new Vector2(0, 0.5f), 
            dir, new Vector2(0, GamePhysics.GravityAcceleration / 2), 8, 12 * sqPower, 0.125f, 0.5f, MathF.PI / 6);
        
        return true;
    }
}