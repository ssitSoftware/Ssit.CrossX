using System;
using Ssit.CrossX.Games.Physics.Collision.Shapes;
using Ssit.CrossX.Games.Physics.Dynamics;
using Ssit.CrossX.Games.Physics.Dynamics.Contacts;

namespace Gunslinger.Core.Game;

public static class GamePhysics
{
    public enum MaterialKind
    {
        Default,
        Platform,
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
            case 5:
            case 6:
                return MaterialKind.Platform;
            default:
                return MaterialKind.Default;
        }
    }
    
    public static void InitPhysicsForWorld(World world)
    {
        world.BodyAdded += WorldOnBodyAdded;
        world.BodyRemoved += WorldOnBodyRemoved;
        world.Disposing += WorldOnDisposing;
    }

    private static void WorldOnBodyAdded(Body body)
    {
        if (GetMaterialKind(body.MaterialIndex) == MaterialKind.Platform)
        {
            body.OnCollision += BodyOnPlatformCollision;
        }
    }
    
    private static void WorldOnBodyRemoved(Body body)
    {
        if (GetMaterialKind(body.MaterialIndex) == MaterialKind.Platform)
        {
            body.OnCollision -= BodyOnPlatformCollision;
        }
    }

    private static void WorldOnDisposing(World world)
    {
        world.BodyAdded -= WorldOnBodyAdded;
        world.BodyRemoved -= WorldOnBodyAdded;
        world.Disposing -= WorldOnDisposing;
    }

    private static bool BodyOnPlatformCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
    {
        if (fixtureB.Body.LinearVelocity.Y < 0) return false;
        
        if (fixtureA.Shape is EdgeShape edge)
        {
            var positionY = MathF.Max(edge.Vertex1.Y + fixtureA.Body.Position.Y, edge.Vertex2.Y + fixtureA.Body.Position.Y);
            
            if (fixtureB.Body.Position.Y > positionY + 0.05f)
            {
                return false;
            }
        }
        
        return true;
    }
}