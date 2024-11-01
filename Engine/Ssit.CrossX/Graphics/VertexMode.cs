using System;

namespace Ssit.CrossX.Graphics;

[Flags]
public enum VertexMode
{
    Invalid = 0,
    Position = 1,
    Color = 2,
    Texture = 4
}