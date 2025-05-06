using System;
using System.Collections.Generic;

namespace Ssit.CrossX.Graphics.Internal;

internal class PaletteSource(RgbaColor[] palette) : IPaletteSource
{
    public event Action OnPaletteChanged;
    public IReadOnlyList<RgbaColor> Palette { get; private set; } = palette;
    public IReadOnlyList<RgbaColor> OriginalPalette { get; } = palette;
    
    public void UpdatePalette(RgbaColor[] palette)
    {
        Palette = palette;
        OnPaletteChanged?.Invoke();
    }
}