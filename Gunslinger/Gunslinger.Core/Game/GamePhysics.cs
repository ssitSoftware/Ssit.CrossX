using Ssit.CrossX.Games.Physics.Dynamics;
using Ssit.CrossX.Games.Physics.Extensions;

namespace Gunslinger.Core.Game;

public static class GamePhysics
{
    public static readonly float[] DefaultKineticFactors = [0.6f, 0.63f, 0.815f, 0.845f, 0.89f, 0.91f, 0.93f, 0.94f, 0.953f, 0.96f, 0.97f];
    
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
    public const float RunAcceleration = 100;
    public const float GroundDeceleration = 45;
    public const float AirBrakeDeceleration = 16;
    public const float AirSteerAcceleration = 32;
    public const float PlayerGroundDistToFall = 2f;
    
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