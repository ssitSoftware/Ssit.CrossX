using System.Collections.Generic;
using System.Numerics;
using Ssit.CrossX.XxGames.Physics;

namespace Ssit.CrossX.XxGames.Logic.Objects.Characters;

public class CharacterSteringParameters
{
    public List<Vector2> InAirVelocity { get; } = [];
    public float InAirSteringFactor;

    public bool CanEnterIdle;
    
    public float WalkTime = 0;
    public int LastDir = 0;
    
    public bool AttackRequested;
    public bool Slam;

    public bool IsOnGround;
    public bool IsOnPlatform;
    public bool IsOnStaticGround;
    public IMaterial GroundMaterial;
    
    public bool CanAttack;
    public bool DoCombo;

    public bool CanPlayLandingSound;
    
    public bool JumpOfRequested;
    public bool JumpRequested;
    
    public float MaxVelocity;
    public bool DisableFall;
    
    public ICollider LadderCollider;
    public bool? ClimbDown;
}