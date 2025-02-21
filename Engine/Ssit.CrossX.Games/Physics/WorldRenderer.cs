using System;
using System.Numerics;
using Ssit.CrossX.Games.Physics.Collision.Shapes;
using Ssit.CrossX.Games.Physics.Dynamics;
using Ssit.CrossX.Games.Template;
using Ssit.CrossX.Graphics.Renderer;

namespace Ssit.CrossX.Games.Physics;

public static class WorldRenderer
{
    private static readonly Vector2[] SCirclePoints;

    static WorldRenderer()
    {
        SCirclePoints = new Vector2[36];
        for(var idx =0; idx < 36; ++idx)
        {
            var angle = idx * MathF.PI / 18;
            var p = new Vector2(MathF.Cos(angle), MathF.Sin(angle));
            SCirclePoints[idx] = p;
        }
    }
    
    public static void Render(IGeometryRenderer renderer, Body body, IGameTemplate gameTemplate)
    {
        foreach (var fixture in body.FixtureList)
        {
            if (fixture.Shape == null)
                continue;
            
            Render(renderer, body, fixture, gameTemplate);
        }
    }
    
    public static void Render(IGeometryRenderer renderer, Body body, Fixture fixture, IGameTemplate gameTemplate)
    {
        var shape = fixture.Shape;
        
        var staticColor = RgbaColor.Yellow;

        if (body.MaterialIndex >= 0 && body.MaterialIndex < gameTemplate.Materials.Length)
        {
            var material = gameTemplate.Materials[body.MaterialIndex];
            staticColor = material.DebugColor;
        }

        if (fixture.IsSensor)
        {
            staticColor = RgbaColor.Violet;
        }
        
        RgbaColor color = body.IsStatic ? staticColor : (body.Awake ? RgbaColor.OrangeRed : RgbaColor.Green);
        
        switch (shape)
        {
            case PolygonShape polygonShape:
                DrawPolygon(body, polygonShape, renderer, color);
                break;
            
            case CircleShape circleShape:
                DrawCircle(body, circleShape, renderer, color);
                break;
            
            case EdgeShape edgeShape:
                DrawEdge(body.Position, edgeShape, renderer, color);
                break;
            
            case ChainShape chainShape:
                DrawChain(body.Position, chainShape, renderer, color);
                break;
        }
    }

    private static void DrawChain(Vector2 position, ChainShape chainShape, IGeometryRenderer renderer, RgbaColor color)
    {
        for(var idx =0; idx < chainShape.Vertices.Count - 1; ++idx)
        {
            renderer.DrawLine(chainShape.Vertices[idx] + position, chainShape.Vertices[idx + 1] + position, color);
        }
    }

    private static void DrawEdge(Vector2 position, EdgeShape edgeShape, IGeometryRenderer renderer, RgbaColor color)
    {
        renderer.DrawLine(edgeShape.Vertex1 + position, edgeShape.Vertex2 + position, color);

        if (edgeShape.HasVertex0)
        {
            renderer.DrawLine(edgeShape.Vertex1 + position, edgeShape.Vertex0 + position, color * 0.5f);
        }
        
        if (edgeShape.HasVertex3)
        {
            renderer.DrawLine(edgeShape.Vertex2 + position, edgeShape.Vertex3 + position, color * 0.5f);
        }
    }

    private static void DrawCircle(Body body, CircleShape circleShape, IGeometryRenderer renderer, RgbaColor color)
    {
        var center = body.Position + circleShape.Position;
        var radius = circleShape.Radius;

        for (var idx = 0; idx < 36; ++idx)
        {
            var p1 = SCirclePoints[idx] * radius + center;
            var p2 = SCirclePoints[(idx + 1) % SCirclePoints.Length] * radius + center;
            renderer.DrawLine(p1, p2, color);
        }
    }

    private static void DrawPolygon(Body body, PolygonShape polygonShape, IGeometryRenderer renderer, RgbaColor color)
    {
        for(var idx =0; idx < polygonShape.Vertices.Count; ++idx)
        {
            renderer.DrawLine(polygonShape.Vertices[idx] + body.Position, polygonShape.Vertices[(idx + 1) % polygonShape.Vertices.Count] + body.Position, color);
        }
    }
}