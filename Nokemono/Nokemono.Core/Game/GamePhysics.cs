using Ssit.CrossX.Games.Physics.Dynamics;
using Ssit.CrossX.Games.Physics.Extensions;

namespace Nokemono.Core.Game;

public static class GamePhysics
{
    public enum MaterialKind
    {
        Default,
        Platform,
        Hurt
    }
    
    public static class Materials
    {
        public const int Any = -1;
        public const int Default = 0;
        public const int Water = 3;
        public const int WoodPlatform = 4;
        public const int MetalPlatform = 5;
        public const int Platform = 6;
        public const int Hurt = 7;
    }
    
    public const float GravityAcceleration = 56;
    public const float JumpVelocity = 11f;
    public const float JumpHoldAccel = 32;
    public const float JumpHoldAccelInc = 2;
    public const float RunSpeed = 12;
    public const float MinRunSpeed = 4;
    public const float RunAccelerationSpeed = 10;
    public const float RunAcceleration = 80;
    public const float AirBrakeDeceleration = 8;
    public const float AirSteerAcceleration = 24;
    public const float PlayerGroundDistToFall = 2f;
    
    public static MaterialKind GetMaterialKind(int material)
    {
        switch (material)
        {
            case Materials.MetalPlatform:
            case Materials.WoodPlatform:
            case Materials.Platform:
                return MaterialKind.Platform;
            
            case Materials.Hurt:
                return MaterialKind.Hurt;
            
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

        if (body.IsStatic)
        {
            body.Friction = 1;
            foreach (var fix in body.FixtureList)
            {
                fix.Shape.Density = 1000;
            }
        }
    }

    private static void WorldOnDisposing(World world)
    {
        world.BodyAdded -= WorldOnBodyAdded;
        world.Disposing -= WorldOnDisposing;
    }
}