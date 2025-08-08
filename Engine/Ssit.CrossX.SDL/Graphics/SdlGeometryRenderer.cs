using System.Numerics;
using SDL;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using static SDL.SDL3;

namespace Ssit.CrossX.SDL.Graphics;

internal unsafe class SdlGeometryRenderer(SDL_Renderer* renderer, IRenderStateProvider renderStateProvider)
    : SdlRendererBase(renderStateProvider), IGeometryRenderer
{
    public int LinesRendered { get; private set; }
    public int RectanglesFilled { get; private set; }

    private SDL_Vertex[] _verticesArray = new SDL_Vertex[1]; 
    
    public void DrawLine(Vector2 v1, Vector2 v2, RgbaColor color)
    {
        var scale = RenderStateProvider.Scale;
        var offset = RenderStateProvider.Offset;
        
        v1 = v1 * scale + offset;
        v2 = v2 * scale + offset;
        
        SDL_SetRenderDrawColor(renderer, color.R, color.G, color.B, color.A);
        SDL_SetRenderDrawBlendMode(renderer, RenderStateProvider.BlendMode.ToSdlBlendMode());
        SDL_RenderLine(renderer, v1.X, v1.Y, v2.X, v2.Y);

        LinesRendered++;
    }

    public void DrawRectangle(RectangleF rect, RgbaColor color)
    {
        var scale = RenderStateProvider.Scale;
        var offset = RenderStateProvider.Offset;
        
        SDL_FRect targetRect = new()
        {
            x = rect.X * scale + offset.X,
            y = rect.Y * scale + offset.Y,
            w = rect.Width * scale,
            h = rect.Height * scale
        };
        
        SDL_SetRenderDrawColor(renderer, color.R, color.G, color.B, color.A);
        SDL_SetRenderDrawBlendMode(renderer, RenderStateProvider.BlendMode.ToSdlBlendMode());
        SDL_RenderRect(renderer, &targetRect);

        LinesRendered += 4;
    }

    public void FillRectangle(RectangleF rect, RgbaColor color)
    {
        var scale = RenderStateProvider.Scale;
        var offset = RenderStateProvider.Offset;
        
        SDL_FRect targetRect = new()
        {
            x = rect.X * scale + offset.X,
            y = rect.Y * scale + offset.Y,
            w = rect.Width * scale,
            h = rect.Height * scale
        };
        
        SDL_SetRenderDrawColor(renderer, color.R, color.G, color.B, color.A);
        SDL_SetRenderDrawBlendMode(renderer, RenderStateProvider.BlendMode.ToSdlBlendMode());
        SDL_RenderFillRect(renderer, &targetRect);
        
        RectanglesFilled++;
    }

    public void DrawVertices(ITexture texture, IReadOnlyList<Vertex> vertices, int count = -1, RgbaColor? colorAttr = null)
    {
        if (count == 0) return;
        count = count >= 0 ? count : vertices.Count;
        
        var textureHandle = PrepareTextureRender(texture, colorAttr);
        var verticesArray = PrepareVertices(vertices, count);
        
        fixed (SDL_Vertex* verticesPtr = verticesArray)
        {
            SDL_RenderGeometry(renderer, textureHandle.Pointer, verticesPtr, count, null, 0);
        }
    }

    private SDL_Vertex[] PrepareVertices(IReadOnlyList<Vertex> vertices, int count)
    {
        if (_verticesArray.Length < count)
        {
            _verticesArray = new SDL_Vertex[count];
        }

        for (var idx = 0; idx < count; ++idx)
        {
            var vert = vertices[idx];
            _verticesArray[idx].color = new SDL_FColor
                {
                    r = vert.Color.Rf,
                    g = vert.Color.Gf,
                    b = vert.Color.Bf,
                    a = vert.Color.Af,
                };
            
            _verticesArray[idx].position = new SDL_FPoint
            {
                x = vert.Position.X,
                y = vert.Position.Y
            };

            _verticesArray[idx].tex_coord = new SDL_FPoint
            {
                x = vert.TexCoord.X,
                y = vert.TexCoord.Y
            };
        }

        return _verticesArray;
    }

    public void ResetStats()
    {
        RectanglesFilled = 0;
        LinesRendered = 0;
    }
}