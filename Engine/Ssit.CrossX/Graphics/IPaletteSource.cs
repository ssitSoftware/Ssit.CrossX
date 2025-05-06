using System;
using System.Collections.Generic;

namespace Ssit.CrossX.Graphics;

public interface IPaletteSource
{
    event Action OnPaletteChanged;
    
    IReadOnlyList<RgbaColor> Palette { get; }
    IReadOnlyList<RgbaColor> OriginalPalette { get; }
    
    void UpdatePalette( RgbaColor[] palette );
}