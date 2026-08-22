using SDL;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.SDL.Common;

namespace Ssit.CrossX.SDL.Graphics;

public interface ISdlPalette: IPlatformPalette
{
    SdlHandle<SDL_Palette> PaletteHandle { get; }
    SdlHandle<SDL_Palette> GlowPaletteHandle { get; }
}