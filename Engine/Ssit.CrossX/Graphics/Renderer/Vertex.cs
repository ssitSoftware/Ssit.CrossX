using System.Numerics;

namespace Ssit.CrossX.Graphics.Renderer;

public struct Vertex(float x, float y, RgbaColor color, float tu = 0, float tv = 0)
{
    public Vector2 Position = new(x, y);
    public Vector2 TexCoord = new(tu, tv);
    public RgbaColor Color = color;
}