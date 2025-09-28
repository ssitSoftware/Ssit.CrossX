using System;
using System.Collections.Generic;
using System.Linq;

namespace Ssit.CrossX.Graphics.Internal;

internal class PaletteSource(RgbaColor[] palette) : IPaletteSource
{
    public event Action OnPaletteChanged;
    public IReadOnlyList<RgbaColor> Palette { get; private set; } = palette;
    public IReadOnlyList<RgbaColor> GlowPalette { get; private set; } = Enumerable.Repeat(RgbaColor.Black, palette.Length).ToArray();
    public IReadOnlyList<RgbaColor> OriginalPalette { get; } = palette;
    public bool HasGlowPalette { get; private set; }
    
    public void UpdatePalette(RgbaColor[] palette, RgbaColor[] glowPalette)
    {
        Palette = palette;
        GlowPalette = glowPalette;
        HasGlowPalette = glowPalette != null && glowPalette.Any(o => o != RgbaColor.Black);
        
        OnPaletteChanged?.Invoke();
    }
}