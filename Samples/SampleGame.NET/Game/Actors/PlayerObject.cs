using System;
using System.Numerics;
using SampleGame.Game.Actors.Player;
using SampleGame.Game.Logic;
using SampleGame.Game.Rendering;
using Ssit.CrossX;
using Ssit.CrossX.Content;
using Ssit.CrossX.Games;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Games.Physics.Dynamics;
using Ssit.CrossX.Games.Rendering;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Sprites;
using Ssit.CrossX.Input;

namespace SampleGame.Game.Actors;

public sealed class PlayerObject: IPhysicsObject, IDrawable, ICamera, ShooterPlayerBrain.IWeaponHandler, IDisposable
{
    private class PlayerObjectData : ObjectData, IHittable
    {
        private readonly PlayerObject _playerObject;

        public PlayerObjectData(IUpdatable updatable, PlayerObject playerObject) : base(updatable, playerObject)
        {
            _playerObject = playerObject;
        }

        public void Hit(float damage, Vector2 direction)
        {
            _playerObject.Hit(damage, direction);
        }
    }

    public IPlayerGun Gun { get; }
    public IPlayerSword Sword { get; } = new MetalTube();
    
    public event Action<float> OnShot;

    private readonly IContentManager _contentManager;
    private readonly Simulation _simulation;
    public Body Body { get; }

    public Vector2 LookAt => Vector2.Zero;
    public ICamera Camera => this;
    
    public bool IsReloading => Gun.IsReloading;
    
    private readonly SpriteShooterRenderer _spriteShooterRenderer;
    private readonly ShooterPlayerBrain _brain;

    private readonly Vector2 _gunOffset = new Vector2(0, -9);
    
    private readonly ResourceHandle<GameObject> _bulletsObj;
    private readonly SpriteInstance _bullets;
    
    public PlayerObject(IContentManager contentManager, IInputMappings inputMappings, Simulation simulation)
    {
        _contentManager = contentManager;
        _simulation = simulation;

        _bulletsObj = contentManager.Get<GameObject>("assets:/Sprites/Bullet");
        _bullets = _bulletsObj.Resource.CreateSpriteInstance();
        
        Gun = new Revolver(contentManager);
        
        using var shadowObj = contentManager.Get<GameObject>("assets:/Sprites/CharacterShadow");
        using var heroObj = contentManager.Get<GameObject>("assets:/Sprites/Hero");
        using var gunObj = contentManager.Get<GameObject>("assets:/Sprites/HeroGun");
        
        _spriteShooterRenderer = new SpriteShooterRenderer(heroObj, gunObj, _gunOffset, shadowObj);
        _brain = new ShooterPlayerBrain(this, this, new PlayerController(inputMappings), _spriteShooterRenderer);
        
        Body = new Body(simulation.World, bodyType: BodyType.Dynamic, userdata: new PlayerObjectData(_brain, this));

        Gun.FastReload();
    }
    
    public void Draw(IRenderer renderer, RenderPass renderPass)
    {
        _spriteShooterRenderer.Render(renderer, Body.Position * _simulation.UnitScale, renderPass);
    }

    public void Dispose()
    {
        _spriteShooterRenderer?.Dispose();
        _bulletsObj?.Dispose();
        _bullets?.Dispose();
        
        Gun?.Dispose();
    }

    public void Shot()
    {
        if (!Gun.Shot())
        {
            return;
        }
        OnShot?.Invoke(Gun.Recoil);

        var aimDir = Vector2.Normalize(_brain.AimDirection);
        var bulletPosition = _brain.Body.Position + _gunOffset / _simulation.UnitScale + aimDir * 16 / _simulation.UnitScale;

        var bulletParams = new BulletObject.Parameters
        {
            Position = bulletPosition,
            Direction = aimDir,
            Speed = 20,
            Damage = 10,
            Range = 20,
            Tails = 8,
            Type = "Small",
            BulletSize = 0.5f,
            Color = RgbaColor.Orange
        };

        // ReSharper disable once ObjectCreationAsStatement
        new BulletObject(_simulation, _contentManager, bulletParams);
    }

    public void Attack()
    {
        if (Gun.IsReloading)
        {
            Gun.CancelReload();
        }
    }

    public void Reload()
    {
        Gun.Reload();
    }

    public void CancelReload()
    {
        Gun.CancelReload();
    }

    public void Update(float dt)
    {
        Gun.Update(dt);
    }
    
    private void Hit(float damage, Vector2 direction)
    {
        
    }
}