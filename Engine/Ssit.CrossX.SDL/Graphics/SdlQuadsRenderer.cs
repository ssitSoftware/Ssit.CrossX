using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using static bottlenoselabs.Interop.SDL;

namespace Ssit.CrossX.SDL.Graphics;

public unsafe class SdlQuadsRenderer(SDL_Renderer* renderer, IRenderStateProvider renderStateProvider)
    : SdlRendererBase(renderStateProvider), IQuadsRenderer
{
    public int QuadsRendered { get; private set; }

    public void Draw(ITexture texture, RectangleF target, Rectangle? nullableSource = null, RgbaColor? colorAttr = null)
    {
        var scale = RenderStateProvider.Scale;
        var offset = RenderStateProvider.Offset;
        
        var source = nullableSource ?? new Rectangle(0, 0, texture.Size.Width, texture.Size.Height);
        
        SDL_FRect sourceRect = new()
        {
            x = source.X,
            y = source.Y,
            w = source.Width,
            h = source.Height
        };


        SDL_FRect targetRect = new()
        {
            x = target.X * scale + offset.X,
            y = target.Y * scale + offset.Y,
            w = target.Width * scale,
            h = target.Height * scale
        };
        
        var textureHandle = PrepareTextureRender(texture, colorAttr);
        
        SDL_RenderTexture(renderer, textureHandle.Pointer,
            &sourceRect, &targetRect);

        QuadsRendered++;
    }

    public void Draw(ITexture texture, IReadOnlyList<Quad> quads, RgbaColor colorAttr)
    {
        var scale = RenderStateProvider.Scale;
        var offset = RenderStateProvider.Offset;
        
        SDL_FRect sourceRect = new();
        SDL_FRect targetRect = new();

        var textureHandle = PrepareTextureRender(texture, colorAttr);
        
        for (var idx = 0; idx < quads.Count; idx++)
        {
            var src = quads[idx].Source;
            var target = quads[idx].Target;

            sourceRect.x = src.X;
            sourceRect.y = src.Y;
            sourceRect.w = src.Width;
            sourceRect.h = src.Height;
            
            targetRect.x = target.X * scale + offset.X;
            targetRect.y = target.Y * scale + offset.Y;
            targetRect.w = target.Width * scale;
            targetRect.h = target.Height * scale;
            
            SDL_RenderTexture(renderer, textureHandle.Pointer,
                &sourceRect, &targetRect);
        }
        
        QuadsRendered += quads.Count;
    }
    
    public void ResetStats() => QuadsRendered = 0;
}