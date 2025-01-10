using System;
using System.Collections.Generic;
using System.Numerics;
using SampleGame.Game.Logic.Behaviors.ShooterPlayer;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Games.Physics.Dynamics;
using Ssit.CrossX.Games.Rendering;
using Ssit.CrossX.Graphics;

namespace SampleGame.Game.Logic;

public class ShooterPlayerBrain : Brain
{
    public interface IWeaponHandler
    {
        event Action<float> OnShot;
        
        bool IsReloading { get; }
        void Shot();
        void Attack();
        void Reload();
        void CancelReload();
        void Update(float dt);
    }
    
    private readonly Dictionary<(string, string), string> _sequences = new();

    private readonly IPhysicsObject _physicsObject;
    public IWeaponHandler WeaponHandler { get; }
    public readonly IGameObjectController Controller;
    private readonly IShooterRenderer _renderer;

    private bool _reverseMove;
    private float _recoil;
    
    public Vector2 AimDirection { get; set; } = new(0,0);
    public Vector2 MoveDirection { get; set; }= new(0,0);
    public Vector2 CharacterDirection { get; set; } = new(0, 1);
    
    public Body Body => _physicsObject.Body;
    
    public ShooterPlayerBrain(IPhysicsObject physicsObject, IWeaponHandler weaponHandler, IGameObjectController controller, IShooterRenderer renderer): base(renderer)
    {
        Controller = controller;
        WeaponHandler = weaponHandler;

        WeaponHandler.OnShot += OnShot;
        
        _physicsObject = physicsObject;
        _renderer = renderer;

        var runBehavior = new RunBehavior(this);
        var shootBehavior = new ShootBehavior(this);
        var swordAttackBehavior = new SwordAttackBehavior(this);
        var attackingBehavior = new AttackingBehavior(this);
        
        
        var rollingBehavior = new RollingBehavior(this);
        var rollBehavior = new RollBehavior(this);
        
        var idleState = new State(runBehavior, swordAttackBehavior, shootBehavior, rollBehavior);
        var runState = new State(runBehavior, swordAttackBehavior, shootBehavior, rollBehavior);
        var chopState = new State(attackingBehavior);
        var rollState = new State(rollingBehavior);
        
        AddState("Idle", idleState);
        AddState("Move", runState);
        AddState("Chop", chopState);
        AddState("Roll", rollState);
        
        SetState("Idle");
    }

    protected override void OnUpdate(float dt)
    {
        base.OnUpdate(dt);
        ProcessVectors();
        WeaponHandler.Update(dt);
        
        Body.Position += MoveDirection * dt;
        _renderer.Animate(dt, _reverseMove);
        
        _recoil -= dt * 10f;
        _recoil = MathF.Max(0, _recoil);
    }
    
    protected string GetSequence(string state, string direction)
    {
        if (_sequences.TryGetValue((state, direction), out var sequence))
        {
            return sequence;
        }
        
        sequence = $"{state} {direction}";
        _sequences.Add((state, direction), sequence);
        return sequence;
    }

    protected override void SetSequence(string state)
    {
        var direction = "Down";
        var transform = ImageTransform.None;
        
        var angle = MathF.Atan2(CharacterDirection.Y, CharacterDirection.X) - MathF.PI / 4;
        
        var angleInt = (int)(angle / (MathF.PI / 8) + 16) % 16;
        
        _renderer.GunBehind = angleInt is >= 7 and <= 12;
        
        switch (angleInt)
        {
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
                direction = "L/R";
                transform = ImageTransform.FlipHorizontal;
                break;
            
            
            case 9:
            case 10:
            case 11:
                direction = "Up";
                break;
            
            case 12:
            case 13:
            case 14:
            case 15:
            case 0:
                direction = "L/R";
                break;
        }
       
        
        _renderer.UpdateRenderState(GetSequence(state, direction), transform);
    }

    private void ProcessVectors()
    {
        CharacterDirection = AimDirection.Length() > 0.1f ? AimDirection :
            MoveDirection.Length() > 0.1f ? MoveDirection : CharacterDirection;
        
        CharacterDirection = Vector2.Normalize(CharacterDirection);
        _reverseMove = Vector2.Dot(CharacterDirection, MoveDirection) < 0;

        bool isAiming = AimDirection.Length() > 0.1f;
        
        var angle = MathF.Atan2(CharacterDirection.Y, CharacterDirection.X) - MathF.PI / 2;
        
        _renderer.UpdateAimingAngle(angle, isAiming, WeaponHandler.IsReloading, _recoil);
    }

    private void OnShot(float recoil)
    {
        if (_recoil == 0)
        {
            _recoil = recoil;
        }
    }
}