using System.Diagnostics;
using System.Numerics;

namespace Ssit.CrossX;

[DebuggerDisplay("Size = ({Width}, {Height})")]
public readonly struct SizeF
{
    public static readonly SizeF Empty = new (0, 0);

    public readonly float Width;
    public readonly float Height;
    
    public SizeF(float width, float height)
    {
        Width = width;
        Height = height;
    }

    public Vector2 ToVector() => new(Width, Height);
}