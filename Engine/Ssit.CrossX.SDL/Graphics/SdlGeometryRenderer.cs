using System.Numerics;
using Ssit.CrossX.Graphics.Renderer;
using static bottlenoselabs.Interop.SDL;

namespace Ssit.CrossX.SDL.Graphics;

internal unsafe class SdlGeometryRenderer(SDL_Renderer* renderer, IRenderStateProvider renderStateProvider)
    : IGeometryRenderer
{
    public int LinesRendered { get; private set; }
    public int RectanglesFilled { get; private set; }
    
    public void DrawLine(Vector2 v1, Vector2 v2, RgbaColor color)
    {
        var scale = renderStateProvider.Scale;
        var offset = renderStateProvider.Offset;
        
        v1 = v1 * scale + offset;
        v2 = v2 * scale + offset;
        
        SDL_SetRenderDrawColor(renderer, color.R, color.G, color.B, color.A);
        SDL_SetRenderDrawBlendMode(renderer, renderStateProvider.BlendMode.ToSdlBlendMode());
        SDL_RenderLine(renderer, v1.X, v1.Y, v2.X, v2.Y);

        LinesRendered++;
    }

    public void DrawRectangle(RectangleF rect, RgbaColor color)
    {
        var scale = renderStateProvider.Scale;
        var offset = renderStateProvider.Offset;
        
        SDL_FRect targetRect = new()
        {
            x = rect.X * scale + offset.X,
            y = rect.Y * scale + offset.Y,
            w = rect.Width * scale,
            h = rect.Height * scale
        };
        
        SDL_SetRenderDrawColor(renderer, color.R, color.G, color.B, color.A);
        SDL_SetRenderDrawBlendMode(renderer, renderStateProvider.BlendMode.ToSdlBlendMode());
        SDL_RenderRect(renderer, &targetRect);

        LinesRendered += 4;
    }

    public void FillRectangle(RectangleF rect, RgbaColor color)
    {
        var scale = renderStateProvider.Scale;
        var offset = renderStateProvider.Offset;
        
        SDL_FRect targetRect = new()
        {
            x = rect.X * scale + offset.X,
            y = rect.Y * scale + offset.Y,
            w = rect.Width * scale,
            h = rect.Height * scale
        };
        
        SDL_SetRenderDrawColor(renderer, color.R, color.G, color.B, color.A);
        SDL_SetRenderDrawBlendMode(renderer, renderStateProvider.BlendMode.ToSdlBlendMode());
        SDL_RenderFillRect(renderer, &targetRect);
        
        RectanglesFilled++;
    }

    public void ResetStats()
    {
        RectanglesFilled = 0;
        LinesRendered = 0;
    }
}