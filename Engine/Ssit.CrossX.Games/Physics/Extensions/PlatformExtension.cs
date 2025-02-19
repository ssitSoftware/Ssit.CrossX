using System;
using Ssit.CrossX.Games.Physics.Collision.Shapes;
using Ssit.CrossX.Games.Physics.Dynamics;
using Ssit.CrossX.Games.Physics.Dynamics.Contacts;

namespace Ssit.CrossX.Games.Physics.Extensions;

public class PlatformExtension: IDisposable
{
    private readonly float _tolerance;
    private readonly Body _body;
    
    public static void Attach(Body body) => Attach(body, 0.05f);
    
    public static void Attach(Body body, float tolerance)
    {
        body.SetExtension(new PlatformExtension(body, tolerance));
    }

    private PlatformExtension(Body body, float tolerance)
    {
        _body = body;
        _tolerance = tolerance;
        
        body.OnCollision += BodyOnOnCollision;
    }
    
    public void Dispose() => _body.OnCollision -= BodyOnOnCollision;

    private static bool BodyOnOnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
    {
        if (fixtureB.Body.LinearVelocity.Y < 0) return false;

        var tolerance = fixtureA.Body.GetExtension<PlatformExtension>()?._tolerance ?? 0.05f;
        
        if (fixtureA.Shape is EdgeShape edge)
        {
            var positionY = MathF.Max(edge.Vertex1.Y + fixtureA.Body.Position.Y, edge.Vertex2.Y + fixtureA.Body.Position.Y);
            
            if (fixtureB.Body.Position.Y > positionY + tolerance)
            {
                return false;
            }
        }
        return true;
    }
}