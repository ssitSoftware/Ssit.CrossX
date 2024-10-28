using System;

namespace Ssit.Pixel.Graphics;

[Flags]
public enum TextureMaps
{
    None = 0,
    Diffuse = 1,
    NormalMap = 2,
    DepthBuffer = 4,
    StencilBuffer = 8
}