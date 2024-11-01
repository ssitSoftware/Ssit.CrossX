using System.Numerics;
using System.Runtime.InteropServices;

namespace Ssit.CrossX.Graphics;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct VertexPositionColorTexture
{
    public const VertexMode Mode = VertexMode.Position | VertexMode.Color | VertexMode.Texture;
    public const int Size = sizeof(float) * 4 + sizeof(byte) * 4 + sizeof(float) * 2;
    
    public Vector4 Position;
    public RgbaColor Color;
    public Vector2 TextureCoordinate;
    
    public VertexPositionColorTexture(Vector3 position, RgbaColor color, Vector2 textureCoordinate)
    {
        Position = new Vector4(position.X, position.Y, position.Z, 1);
        Color = color;
        TextureCoordinate = textureCoordinate;
    }
}