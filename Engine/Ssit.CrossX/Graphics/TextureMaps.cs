using System;

namespace Ssit.CrossX.Graphics;

[Flags]
public enum TextureMaps
{
    None = 0,
    Diffuse = 1,
    NormalMap = 2,
    GlowMap = 4,
    DepthBuffer = 8,
    StencilBuffer = 16
}