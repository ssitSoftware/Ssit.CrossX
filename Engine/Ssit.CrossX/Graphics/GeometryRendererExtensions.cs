using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using Ssit.CrossX.Graphics.Renderer;

namespace Ssit.CrossX.Graphics;

public static class GemoetryRendererExtensions
{
    private static readonly List<Vertex> Vertices = new();
    
    public static void DrawLines(this IGeometryRenderer renderer, IReadOnlyList<Vertex> lines)
    {
        for (var idx = 0; idx < lines.Count; idx+=2)
        {
            renderer.DrawLine(lines[idx].Position, lines[idx + 1].Position, lines[idx].Color);
        }
    }
    
    public static void DrawCircle(this IGeometryRenderer renderer, Vector2 center, float radius, Color color, int steps = 36)
    {
        for (var i = 0; i < steps; i++)
        {
            var angle0 = i * 2f * MathF.PI / steps;
            var angle1 = (i + 1) * 2f * MathF.PI / steps;

            var x0 = radius * MathF.Cos(angle0) + center.X;
            var y0 = radius * MathF.Sin(angle0) + center.Y;

            var x1 = radius * MathF.Cos(angle1) + center.X;
            var y1 = radius * MathF.Sin(angle1) + center.Y;

            renderer.DrawLine(new Vector2(x0, y0), new Vector2(x1, y1), color);
        }
    }
    
    public static void FillCircle(this IGeometryRenderer renderer, Vector2 center, float radius, Color color, int steps = 36)
    {
        Vertices.Clear();

        for (var i = 0; i < steps; i++)
        {
            var angle0 = i * 2f * MathF.PI / steps;
            var angle1 = (i+1) * 2f * MathF.PI / steps;
            
            var x0 = radius * MathF.Cos(angle0) + center.X;
            var y0 = radius * MathF.Sin(angle0) + center.Y;
            
            var x1 = radius * MathF.Cos(angle1) + center.X;
            var y1 = radius * MathF.Sin(angle1) + center.Y;
            
            Vertices.Add(new Vertex
            {
                Position = new Vector2(x0, y0),
                Color = color
            });
            
            Vertices.Add(new Vertex
            {
                Position = new Vector2(x1, y1),
                Color = color
            });
            
            Vertices.Add(new Vertex
            {
                Position = center,
                Color = color
            });
        }
        
        renderer.DrawTriangles(Vertices);
    }
}