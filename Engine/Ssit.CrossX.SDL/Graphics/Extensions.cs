using System.Numerics;
using SDL;
using SkiaSharp;
using Ssit.CrossX.Graphics.Renderer;

namespace Ssit.CrossX.SDL.Graphics;

public static class Extensions
{
    public static float DistanceTo(this SKColor color, RgbaColor other)
    {
        var dr = Math.Abs(color.Red - other.R);
        var dg = Math.Abs(color.Green - other.G);
        var db = Math.Abs(color.Blue - other.B);

        return MathF.Sqrt(dr * dr + dg * dg + db * db);
    }
    
    public static SDL_Vertex[] PrepareVertices(IReadOnlyList<Vertex> vertices, int count, IRenderStateProvider renderStateProvider = null, SDL_Vertex[] array = null)
    {
        var scale = renderStateProvider?.Scale ?? 1f;
        var offset = renderStateProvider?.Offset ?? Vector2.Zero;
        
        if (array is null || array.Length < count)
        {
            array = new SDL_Vertex[count];
        }

        for (var idx = 0; idx < count; ++idx)
        {
            var vert = vertices[idx];
            array[idx].color = new SDL_FColor
            {
                r = vert.Color.Rf,
                g = vert.Color.Gf,
                b = vert.Color.Bf,
                a = vert.Color.Af,
            };
            
            array[idx].position = new SDL_FPoint
            {
                x = vert.Position.X * scale + offset.X,
                y = vert.Position.Y * scale + offset.Y
            };

            array[idx].tex_coord = new SDL_FPoint
            {
                x = vert.TexCoord.X,
                y = vert.TexCoord.Y
            };
        }

        return array;
    }
}