using Ssit.CrossX.SDL.Common;

namespace Ssit.CrossX.SDL.Graphics;

public interface ISdlPalette
{
    event Action OnPaletteChanged;
    public RgbaColor[] OriginalPalette { get; }
    
    SdlHandle<bottlenoselabs.Interop.SDL.SDL_Palette> PaletteHandle { get; }
}