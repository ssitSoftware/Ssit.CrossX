using System;
using System.Collections.Generic;
using System.Numerics;
using Ssit.CrossX.Games.Physics.Collision;
using Ssit.CrossX.Games.Physics.Dynamics;

namespace Ssit.CrossX.Games.Physics.Extensions;

public class MovingStackExtension: IDisposable
{
    private readonly Body _body;
    private readonly Aabb _detectionAabb;
    private readonly float[] _kineticFactors;

    private readonly List<Fixture> _lyingFixtures = new();
    private readonly List<Body> _lyingBodies = new();
    private Vector2 _offset;
    
    public static void Attach(Body body, Aabb detectionAabb, float[] kineticFactors)
    {
        body.SetExtension(new MovingStackExtension(body, detectionAabb, kineticFactors));
    }

    private MovingStackExtension(Body body, Aabb detectionAabb, float[] kineticFactors)
    {
        _body = body;
        _detectionAabb = detectionAabb;
        _kineticFactors = kineticFactors;

        _body.Moved += BodyOnMoved;
        _body.PreProcessing += BodyOnPreProcessing;
        _body.PostProcessing += BodyOnPostProcessing;
    }

    private static void BodyOnPostProcessing(Body body, float dt)
    {
        var ext = body.GetExtension<MovingStackExtension>();

        foreach (var bd in ext._lyingBodies)
        {
            var offset = ext._offset;
            if (bd.Owner is IMomentumReceiver receiver)
            {
                var speed = (int)MathF.Floor(MathF.Abs(offset.X) / dt + 0.05f);
                speed = Math.Min(speed, ext._kineticFactors.Length - 1);
                var kineticFactor = ext._kineticFactors[speed];

                receiver.OnKineticallyMoved(offset, kineticFactor);
            }
            
            var bde = bd.GetExtension<MovingStackExtension>();
            if (bde != null && bde._lyingBodies.Count > 0 && !bde._lyingBodies.Contains(body))
            {
                BodyOnPostProcessing(bd, dt);
            }
        }
        
        ext._offset = Vector2.Zero;
    }
    
    private static void BodyOnPreProcessing(Body body, float dt)
    {
        var ext = body.GetExtension<MovingStackExtension>();

        ext._lyingBodies.Clear();
        
        var p1 = ext._detectionAabb.LowerBound + body.Position;
        var p2 = ext._detectionAabb.UpperBound + body.Position;
        
        var aabb = new Aabb(p1, p2);
        
        body._world.QueryAabbs(ext._lyingFixtures, ref aabb);
        
        ext._lyingBodies.Clear();

        foreach (var fix in ext._lyingFixtures)
        {
            if (fix.Body == body) continue;
            if (fix.Body.IsStatic) continue;
            if (fix.Body.IsKinematic) continue;
            if (fix.Body.Position.Y > body.Position.Y) continue;
            if (ext._lyingBodies.Contains(fix.Body)) continue;
            if (fix.Body.Owner is not IMomentumReceiver) continue;
            
            ext._lyingBodies.Add(fix.Body);
        }
        
        ext._lyingFixtures.Clear();
        ext._offset = Vector2.Zero;
    }

    private static void BodyOnMoved(Body body, Vector2 vector)
    {
        var ext = body.GetExtension<MovingStackExtension>();
        ext._offset += vector;
    }

    public void Dispose()
    {
        _body.Moved -= BodyOnMoved;
        _body.PreProcessing -= BodyOnPreProcessing;
        _body.PostProcessing -= BodyOnPostProcessing;
    }
}