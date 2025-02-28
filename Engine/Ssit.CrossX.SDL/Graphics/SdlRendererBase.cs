using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.SDL.Common;

namespace Ssit.CrossX.SDL.Graphics;

public abstract unsafe class SdlRendererBase(IRenderStateProvider renderStateProvider)
{
    protected readonly IRenderStateProvider RenderStateProvider = renderStateProvider;

    protected SdlHandle<bottlenoselabs.Interop.SDL.SDL_Texture> PrepareTextureRender(ITexture texture, RgbaColor? colorAttr)
    {
        var (textureHandle, color) = RenderStateProvider.GetProperTextureAndColor(texture, colorAttr ?? RgbaColor.White);
        
        bottlenoselabs.Interop.SDL.SDL_SetTextureColorMod(textureHandle.Pointer, color.R, color.G, color.B);
        if (RenderStateProvider.BlendMode == BlendMode.AlphaBlend)
        {
            bottlenoselabs.Interop.SDL.SDL_SetTextureAlphaMod(textureHandle.Pointer, color.A);
        }
        else
        {
            bottlenoselabs.Interop.SDL.SDL_SetTextureAlphaMod(textureHandle.Pointer, 255);
        }

        bottlenoselabs.Interop.SDL.SDL_SetTextureBlendMode(textureHandle.Pointer, RenderStateProvider.BlendMode.ToSdlBlendMode());
        bottlenoselabs.Interop.SDL.SDL_SetTextureScaleMode(textureHandle.Pointer, RenderStateProvider.TextureFilter.ToSdlScaleMode());
        
        return textureHandle;
    }
    
}