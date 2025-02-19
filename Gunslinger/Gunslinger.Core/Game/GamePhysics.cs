using System;
using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.Games.Physics.Collision.Shapes;
using Ssit.CrossX.Games.Physics.Dynamics;
using Ssit.CrossX.Games.Physics.Dynamics.Contacts;

namespace Gunslinger.Core.Game;

public static class GamePhysics
{
    public static void InitPhysicsForWorld(WorldBuilder builder)
    {
        builder.WithMaterialAction("Wood Platform", ConfigurePlatformCollision);
        builder.WithMaterialAction("Metal Platform", ConfigurePlatformCollision);
    }

    public static void DeinitPhysicsForWorld(World world)
    {
        if ( world is null ) return;
        
        foreach (var body in world.BodyList)
        {
            if (body.IsStatic)
            {
                body.OnCollision -= BodyOnPlatformCollision;
            }
        }
    }

    private static void ConfigurePlatformCollision(Body body)
    {
        body.OnCollision += BodyOnPlatformCollision;
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