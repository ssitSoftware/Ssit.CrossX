namespace Gunslinger.Core.Game;

public static class GamePhysicsParameters
{
    public const float GravityAcceleration = 56;
    public const float JumpVelocity = 11f;
    public const float JumpHoldAccel = 32;
    public const float JumpHoldAccelInc = 2;
    public const float RunSpeed = 12;
    public const float MinRunSpeed = 4;
    public const float RunAccelerationSpeed = 10;
    public const float RunAcceleration = 50;
    public const float GroundDeceleration = 60;
    public const float AirBrakeDeceleration = 20;
    public const float AirSteerAcceleration = 40;
    public const float PlayerFriction = 0.01f;
    public const float PlayerVelocityToFall = 10f;

    public static bool IsPlatform(int material)
    {
        return material is 5 or 6;
    }
    
    // public static readonly Material[] Materials =
    // [
    //     new Material{ Friction = 1, Restitution = 0, Platform = false},
    //     new Material{ Friction = 1, Restitution = 0, Platform = false},
    //     new Material{ Friction = 1, Restitution = 0, Platform = false},
    //     new Material{ Friction = 3, Restitution = 0, Platform = false},
    //     new Material{ Friction = 0.1f, Restitution = 0, Platform = false},
    //     new Material{ Friction = 1, Restitution = 0, Platform = true},
    //     new Material{ Friction = 1, Restitution = 0, Platform = true}
    // ];
}