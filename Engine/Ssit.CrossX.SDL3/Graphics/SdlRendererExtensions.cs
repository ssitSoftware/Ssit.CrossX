using bottlenoselabs.Interop;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.SDL3.Common;

namespace Ssit.CrossX.SDL3.Graphics;

public static class SdlRendererExtensions
{
    public static (SdlHandle<SDL.SDL_Texture>, RgbaColor) GetProperTextureAndColor(this IRenderStateProvider renderStateProvider, ITexture texture, RgbaColor? color)
    {
        if (renderStateProvider.UseGlowTextures)
        {
            var map = texture.GetMap<SdlHandle<SDL.SDL_Texture>>(TextureMaps.GlowMap);
            color = color ?? RgbaColor.White;
            
            if (map is null)
            {
                map = texture.GetMap<SdlHandle<SDL.SDL_Texture>>(TextureMaps.Diffuse);
                color = RgbaColor.Black;
            }
            return (map, color.Value);
        }
        
        return (texture.GetMap<SdlHandle<SDL.SDL_Texture>>(TextureMaps.Diffuse), color ?? RgbaColor.White);
    }
}