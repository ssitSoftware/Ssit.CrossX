using System;
using Ssit.CrossX.Games.Physics.Collision.Shapes;
using Ssit.CrossX.Games.Physics.Dynamics;
using Ssit.CrossX.Games.Physics.Dynamics.Contacts;
using Ssit.CrossX.Games.Physics.Extensions;

namespace Gunslinger.Core.Game;

public static class GamePhysics
{
    public enum MaterialKind
    {
        Default,
        Platform,
    }
    
    public static class Materials
    {
        public const int Default = 0;
        public const int WoodPlatform = 5;
        public const int MetalPlatform = 6;
    }
    
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
    public const float PlayerVelocityToFall = 10f;
    
    public static MaterialKind GetMaterialKind(int material)
    {
        switch (material)
        {
            case Materials.MetalPlatform:
            case Materials.WoodPlatform:
                return MaterialKind.Platform;
            default:
                return MaterialKind.Default;
        }
    }
    
    public static void InitPhysicsForWorld(World world)
    {
        world.BodyAdded += WorldOnBodyAdded;
        world.Disposing += WorldOnDisposing;
    }

    private static void WorldOnBodyAdded(Body body)
    {
        if (GetMaterialKind(body.MaterialIndex) == MaterialKind.Platform && body.IsStatic)
        {
            PlatformExtension.Attach(body);
        }
    }

    private static void WorldOnDisposing(World world)
    {
        world.BodyAdded -= WorldOnBodyAdded;
        world.Disposing -= WorldOnDisposing;
    }
}