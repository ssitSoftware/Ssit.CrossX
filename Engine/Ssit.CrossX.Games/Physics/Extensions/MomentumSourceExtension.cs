using System;
using System.Collections.Generic;
using System.Numerics;
using Ssit.CrossX.Games.Physics.Collision;
using Ssit.CrossX.Games.Physics.Dynamics;

namespace Ssit.CrossX.Games.Physics.Extensions;

public class MomentumSourceExtension: IDisposable
{
    private readonly Body _body;
    private readonly Aabb _detectionAabb;

    private readonly List<Fixture> _lyingFixtures = new();
    private readonly List<Body> _lyingBodies = new();
    private Vector2 _offset;
    
    public static void Attach(Body body, Aabb detectionAabb)
    {
        body.SetExtension(new MomentumSourceExtension(body, detectionAabb));
    }

    private MomentumSourceExtension(Body body, Aabb detectionAabb)
    {
        _body = body;
        _detectionAabb = detectionAabb;

        _body.Moved += BodyOnMoved;
        _body.PreProcessing += BodyOnPreProcessing;
        _body.PostProcessing += BodyOnPostProcessing;
    }

    private static void BodyOnPostProcessing(Body body, float dt)
    {
        var ext = body.GetExtension<MomentumSourceExtension>();

        foreach (var bd in ext._lyingBodies)
        {
            var offset = ext._offset;
            if (bd.Owner is IMomentumReceiver receiver)
            {
                receiver.OnMomentumPassed(offset);
            }
        }
        
        ext._offset = Vector2.Zero;
    }
    
    private static void BodyOnPreProcessing(Body body, float dt)
    {
        var ext = body.GetExtension<MomentumSourceExtension>();

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
        var ext = body.GetExtension<MomentumSourceExtension>();
        ext._offset += vector;
    }

    public void Dispose()
    {
        _body.Moved -= BodyOnMoved;
        _body.PreProcessing -= BodyOnPreProcessing;
        _body.PostProcessing -= BodyOnPostProcessing;
    }
}