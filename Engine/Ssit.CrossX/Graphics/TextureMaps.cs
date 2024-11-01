using System;

namespace Ssit.CrossX.Graphics;

[Flags]
public enum TextureMaps
{
    None = 0,
    Diffuse = 1,
    NormalMap = 2,
    DepthBuffer = 4,
    StencilBuffer = 8
}