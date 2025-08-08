using SDL;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.SDL.Common;

using static SDL.SDL3;

namespace Ssit.CrossX.SDL.Graphics;

public abstract unsafe class SdlRendererBase(IRenderStateProvider renderStateProvider)
{
    protected readonly IRenderStateProvider RenderStateProvider = renderStateProvider;

    protected SdlHandle<SDL_Texture> PrepareTextureRender(ITexture texture, RgbaColor? colorAttr)
    {
        var (textureHandle, color) = RenderStateProvider.GetProperTextureAndColor(texture, colorAttr ?? RgbaColor.White);
        
        SDL_SetTextureColorMod(textureHandle.Pointer, color.R, color.G, color.B);
        if (RenderStateProvider.BlendMode == BlendMode.AlphaBlend)
        {
            SDL_SetTextureAlphaMod(textureHandle.Pointer, color.A);
        }
        else
        {
            SDL_SetTextureAlphaMod(textureHandle.Pointer, 255);
        }

        SDL_SetTextureBlendMode(textureHandle.Pointer, RenderStateProvider.BlendMode.ToSdlBlendMode());
        SDL_SetTextureScaleMode(textureHandle.Pointer, RenderStateProvider.TextureFilter.ToSdlScaleMode());
        
        return textureHandle;
    }
}