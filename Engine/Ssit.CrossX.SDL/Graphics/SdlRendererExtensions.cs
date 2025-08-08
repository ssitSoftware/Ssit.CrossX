using SDL;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.SDL.Common;
using static SDL.SDL3;

namespace Ssit.CrossX.SDL.Graphics;

public static class SdlRendererExtensions
{
    public static (SdlHandle<SDL_Texture>, RgbaColor) GetProperTextureAndColor(this IRenderStateProvider renderStateProvider, ITexture texture, RgbaColor? color)
    {
        if (renderStateProvider.UseGlowTextures)
        {
            var map = texture.GetMap<SdlHandle<SDL_Texture>>(TextureMaps.GlowMap);
            color = color ?? RgbaColor.White;
            
            if (map is null)
            {
                map = texture.GetMap<SdlHandle<SDL_Texture>>(TextureMaps.Diffuse);
                color = RgbaColor.Black;
            }
            return (map, color.Value);
        }
        
        return (texture.GetMap<SdlHandle<SDL_Texture>>(TextureMaps.Diffuse), color ?? RgbaColor.White);
    }

    public static SDL_BlendMode ToSdlBlendMode(this BlendMode mode)
    {
        switch (mode)
        {
            case BlendMode.AlphaBlend:
                return SDL_BlendMode.SDL_BLENDMODE_BLEND;
            
            case BlendMode.Additive:
                return SDL_BlendMode.SDL_BLENDMODE_ADD;;
            
            case BlendMode.Multiply:
                return SDL_BlendMode.SDL_BLENDMODE_MUL;;
        }
        return SDL_BLENDMODE_NONE;
    }

    public static SDL_ScaleMode ToSdlScaleMode(this TextureFilter filter)
    {
        switch (filter)
        {
            case TextureFilter.Linear:
                return SDL_ScaleMode.SDL_SCALEMODE_LINEAR;
        }

        return SDL_ScaleMode.SDL_SCALEMODE_NEAREST;
    }
}