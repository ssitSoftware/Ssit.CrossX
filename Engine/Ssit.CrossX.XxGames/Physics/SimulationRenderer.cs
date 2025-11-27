using System.Collections.Generic;
using System.Numerics;
using Ssit.CrossX.Graphics.Renderer;

namespace Ssit.CrossX.XxGames.Physics;

public static class SimulationRenderer
{
    private static readonly List<ICollider> Colliders = new();
    private static readonly List<Aabb> Aabbs = new();

    public static float RenderScale = 1;
    
    private static void DrawRectangle(IGeometryRenderer renderer, RectangleF rect, RgbaColor color)
    {
        renderer.DrawRectangle(rect, color);
    }

    private static void DrawLine(IGeometryRenderer renderer, Vector2 start, Vector2 end, RgbaColor color)
    {
        var px = 1 / RenderScale;
        renderer.DrawLine(start, end, color);
        renderer.DrawLine(start + new Vector2(px, px), end + new Vector2(px, px), color);
    }
    
    public static void Render(IGeometryRenderer renderer, ISimulation simulation)
    {
        var bounds = simulation.Bounds;
        
        Aabbs.Clear();
        simulation.Debug_GetQuadTreeAreas(Aabbs);

        foreach (var aabb in Aabbs)
        {
            DrawRectangle(renderer, (RectangleF)aabb, RgbaColor.Gray * 0.2f);
        }
        
        Colliders.Clear();
        simulation.GetColliders(bounds, Colliders);

        foreach (var collider in Colliders)
        {
            var aabb = collider.GetAabb(collider.AttachedBody?.Position ?? Vector2.Zero);

            var color = collider.Type switch
            {
                ColliderType.Dynamic => (collider.AttachedBody?.IsActive ?? false) ? RgbaColor.Red : RgbaColor.Green,
                ColliderType.Trigger => RgbaColor.Violet,
                ColliderType.Particle => RgbaColor.Gray,
                _ => RgbaColor.Yellow
            };

            if (!collider.IsActive)
            {
                color *= 0.25f;
            }

            DrawLine(renderer, new Vector2(aabb.Left, aabb.Top), new Vector2(aabb.Right, aabb.Top),
                (collider.Material.Sides & ColliderSides.Top) != 0 ? color : color * 0.25f);
            
            DrawLine(renderer, new Vector2(aabb.Left, aabb.Bottom), new Vector2(aabb.Right, aabb.Bottom),
                (collider.Material.Sides & ColliderSides.Bottom) != 0 ? color : color * 0.25f);
            
            DrawLine(renderer, new Vector2(aabb.Left, aabb.Top), new Vector2(aabb.Left, aabb.Bottom),
                (collider.Material.Sides & ColliderSides.Left) != 0 ? color : color * 0.25f);
            
            DrawLine(renderer, new Vector2(aabb.Right, aabb.Top), new Vector2(aabb.Right, aabb.Bottom),
                (collider.Material.Sides & ColliderSides.Right) != 0 ? color : color * 0.25f);
        }
    }
}