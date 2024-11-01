using System.Numerics;
using System.Runtime.InteropServices;

namespace Ssit.CrossX.Graphics;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct VertexPositionColor
{
    public const VertexMode Mode = VertexMode.Position | VertexMode.Color;
    public const int Size = sizeof(float) * 4 + sizeof(byte) * 4;
    
    public Vector4 Position;
    public RgbaColor Color;

    public VertexPositionColor(Vector3 position, RgbaColor color)
    {
        Position = new Vector4(position.X, position.Y, position.Z, 1);
        Color = color;
    }
}