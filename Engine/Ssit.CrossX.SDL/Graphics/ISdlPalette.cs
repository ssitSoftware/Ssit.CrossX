using System;
using SDL;
using Ssit.CrossX.SDL.Common;

namespace Ssit.CrossX.SDL.Graphics;

public interface ISdlPalette
{
    event Action OnPaletteChanged;
    public RgbaColor[] OriginalPalette { get; }
    
    SdlHandle<SDL_Palette> PaletteHandle { get; }
    SdlHandle<SDL_Palette> GlowPaletteHandle { get; }
    
    bool HasGlowPalette { get; }
}