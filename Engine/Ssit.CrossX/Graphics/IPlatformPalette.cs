using System;

namespace Ssit.CrossX.Graphics;

public interface IPlatformPalette
{
    event Action OnPaletteChanged;
    RgbaColor[] OriginalPalette { get; }
    bool HasGlowPalette { get; }
}