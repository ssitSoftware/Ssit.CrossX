namespace Ssit.CrossX.XxGames.Logic.Objects.Characters;

public interface ICharacterPhysicsValues
{
    float RunSpeed => 10f;
    float Acceleration => 44f;
    float JumpVelocity => 12f;
    float WalkSpeed => 4.4f;
    float JumpFactor => 4.0f;
    float JumpOfSpeed => 2.0f;
    float PushPullVelocity => 4.4f;
    float FrictionOnLandingFactor => 1.1f;
    float JumpHoldAccelFactor => 2.0f;
    float JumpHoldAccelInc => 0.11f;
    float WallSlideSpeed => 5.5f;
    float WallSlideTimeout => 0.44f;
    float WallClimbSpeed => 8.8f;
    float AttackVelocity => 13.2f;
    float JumpAttackRaiseVelocity => JumpVelocity * 1.25f;
    float AirAttackDownVelocity => 15f;
    float ThrustDownVelocity => 18f;
}