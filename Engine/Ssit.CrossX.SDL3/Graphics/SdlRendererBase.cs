using bottlenoselabs.Interop;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.SDL3.Common;

namespace Ssit.CrossX.SDL3.Graphics;

public abstract unsafe class SdlRendererBase(IRenderStateProvider renderStateProvider)
{
    protected readonly IRenderStateProvider RenderStateProvider = renderStateProvider;

    protected SdlHandle<SDL.SDL_Texture> PrepareTextureRender(ITexture texture, RgbaColor? colorAttr)
    {
        var (textureHandle, color) = RenderStateProvider.GetProperTextureAndColor(texture, colorAttr ?? RgbaColor.White);
        
        SDL.SDL_SetTextureColorMod(textureHandle.Pointer, color.R, color.G, color.B);
        if (RenderStateProvider.BlendMode == BlendMode.AlphaBlend)
        {
            SDL.SDL_SetTextureAlphaMod(textureHandle.Pointer, color.A);
        }
        else
        {
            SDL.SDL_SetTextureAlphaMod(textureHandle.Pointer, 255);
        }

        SDL.SDL_SetTextureBlendMode(textureHandle.Pointer, RenderStateProvider.BlendMode.ToSdlBlendMode());
        SDL.SDL_SetTextureScaleMode(textureHandle.Pointer, RenderStateProvider.TextureFilter.ToSdlScaleMode());
        
        return textureHandle;
    }
    
}