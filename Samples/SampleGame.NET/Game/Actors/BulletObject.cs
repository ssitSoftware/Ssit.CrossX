using System;
using System.Numerics;
using SampleGame.Game.Logic;
using Ssit.CrossX;
using Ssit.CrossX.Content;
using Ssit.CrossX.Games;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Games.Physics.Collision.Shapes;
using Ssit.CrossX.Games.Physics.Dynamics;
using Ssit.CrossX.Games.Physics.Dynamics.Contacts;
using Ssit.CrossX.Games.Rendering;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Sprites;

namespace SampleGame.Game.Actors;

public class BulletObject: IDrawable, IUpdatable, IDisposable
{
    public class Parameters
    {
        public string Type;
        public Vector2 Position;
        public Vector2 Direction;
        public float Speed;
        public float Damage;
        public float Range;
        public int Tails;
        public float BulletSize;
        public RgbaColor Color;
    }
    
    public readonly Body Body;
    
    private readonly Simulation _simulation;
    private readonly SpriteInstance _sprite;

    private readonly Parameters _parameters;

    private readonly Rectangle _bulletSource;
    private readonly Rectangle _tailSource;

    private Vector2 _bulletOrigin;
    private Vector2 _tailOrigin;
    
    public BulletObject(Simulation simulation, IContentManager contentManager, Parameters parameters)
    {
        _parameters = parameters;
        _simulation = simulation;
        
        using var obj = contentManager.Get<GameObject>("assets:/Sprites/Bullet");
        _sprite = obj.Resource.CreateSpriteInstance();
        
        _sprite.SetSequence(_parameters.Type);

        _bulletSource = _sprite.Source;
        _bulletOrigin = _sprite.Origin;
        
        try
        {
            _sprite.SetSequence(_parameters.Type + " Tail");
        }
        catch
        {
            _sprite.SetSequence(_parameters.Type);
        }
        
        _tailSource = _sprite.Source;
        _tailOrigin = _sprite.Origin;
        
        Body = new Body(simulation.World, bodyType: BodyType.Dynamic);
        Body.IsBullet = true;
        Body.CreateFixture(new CircleShape(_parameters.BulletSize / 2, 0.00001f));
        
        Body.Position = _parameters.Position;
        Body.LinearVelocity = _parameters.Direction * _parameters.Speed;
        Body.UserData = this;
        
        Body.OnCollision += BodyOnOnCollision;
    }

    private bool BodyOnOnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
    {
        var body = fixtureA.Body == Body ? fixtureB.Body : fixtureA.Body;

        if (body.UserData is IHittable hittable)
        {
            hittable.Hit(_parameters.Damage, _parameters.Direction);
        }
        _simulation.World.RemoveBody(Body);
        return true;
    }

    public void Draw(IRenderer renderer, RenderPass renderPass)
    {
        if (renderPass != RenderPass.Normal)
            return;

        var dist = (Body.Position - _parameters.Position).Length();
        var bulletOpacity = MathF.Sqrt(MathF.Sqrt(1 - MathF.Min(1, dist / _parameters.Range)));
        
        for (var idx = 0; idx < _parameters.Tails; idx++)
        {
            var offset = _parameters.Direction * (_parameters.Tails - idx) * _parameters.Speed * 0.1f;
            var opacity = idx / (float)(_parameters.Tails + 1);
            renderer.DrawTexture(_sprite.SpriteSheet, Body.Position * _simulation.UnitScale - offset, _tailSource, _tailOrigin, scale: opacity, color: _parameters.Color * opacity * bulletOpacity);
        }
        
        renderer.DrawTexture(_sprite.SpriteSheet, Body.Position * _simulation.UnitScale, _bulletSource, _bulletOrigin, color: _parameters.Color * bulletOpacity);
    }

    public void PostFixedUpdate()
    {
        if ((Body.Position - _parameters.Position).Length() >= _parameters.Range)
        {
            _simulation.World.RemoveBody(Body);
        }
    }

    public void Dispose()
    {
        _sprite?.Dispose();
    }
}