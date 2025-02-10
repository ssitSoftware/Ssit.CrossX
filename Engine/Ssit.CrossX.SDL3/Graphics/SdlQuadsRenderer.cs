using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using static bottlenoselabs.Interop.SDL;

namespace Ssit.CrossX.SDL3.Graphics;

public unsafe class SdlQuadsRenderer(SDL_Renderer* renderer, IRenderStateProvider renderStateProvider)
    : IQuadsRenderer
{
    public void Draw(ITexture texture, RectangleF target, Rectangle source, RgbaColor colorAttr)
    {
        var scale = renderStateProvider.Scale;
        var offset = renderStateProvider.Offset;
        
        var (textureHandle, color) = renderStateProvider.GetProperTextureAndColor(texture, colorAttr);
        
        SDL_FRect sourceRect = new()
        {
            x = source.X * scale + offset.X,
            y = source.Y * scale + offset.Y,
            w = source.Width * scale,
            h = source.Height * scale
        };


        SDL_FRect targetRect = new()
        {
            x = target.X * scale + offset.X,
            y = target.Y * scale + offset.Y,
            w = target.Width * scale,
            h = target.Height * scale
        };
        
        SDL_SetRenderDrawColor(renderer, color.R, color.G, color.B, color.A);
        SDL_RenderTexture(renderer, textureHandle.Pointer,
            &sourceRect, &targetRect);
    }

    public void Draw(ITexture texture, IReadOnlyList<Quad> quads, RgbaColor colorAttr)
    {
        var scale = renderStateProvider.Scale;
        var offset = renderStateProvider.Offset;
        
        SDL_FRect sourceRect = new();
        SDL_FRect targetRect = new();

        var (textureHandle, color) = renderStateProvider.GetProperTextureAndColor(texture, colorAttr);
        
        SDL_SetRenderDrawColor(renderer, color.R, color.G, color.B, color.A);
        
        for (var idx = 0; idx < quads.Count; idx++)
        {
            var src = quads[idx].Source;
            var target = quads[idx].Target;

            sourceRect.x = src.X * scale + offset.X;
            sourceRect.y = src.Y * scale + offset.Y;
            sourceRect.w = src.Width * scale;
            sourceRect.h = src.Height * scale;
            
            targetRect.x = target.X * scale + offset.X;
            targetRect.y = target.Y * scale + offset.Y;
            targetRect.w = target.Width * scale;
            targetRect.h = target.Height * scale;
            
            SDL_RenderTexture(renderer, textureHandle.Pointer,
                &sourceRect, &targetRect);
        }
    }
}