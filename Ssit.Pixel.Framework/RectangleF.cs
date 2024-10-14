using System;
using System.Diagnostics;
using System.Numerics;

namespace Ssit.Pixel.Framework;

[DebuggerDisplay("RectangleF = ({X}, {Y}, {Width}, {Height})")]
public readonly struct RectangleF
{
    public RectangleF(Vector2 position, SizeF size)
    {
        X = position.X;
        Y = position.Y;

        Width = size.Width;
        Height = size.Height;
    }
    
    public RectangleF(float x, float y, float width, float height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public Vector2 TopLeft => new(X, Y);
    public Vector2 BottomRight => new(Right, Bottom);
    
    public Vector2 BottomLeft => new(X, Bottom);
    public Vector2 TopRight => new(Right, Y);

    public Vector2 Center => new(X + Width / 2, Y + Height / 2);

    public SizeF Size => new(Width, Height);
    
    public readonly float X;
    public readonly float Y;

    public readonly float Width;
    public readonly float Height;

    public float Right => X + Width;
    public float Bottom => Y + Height;

    public bool IsEmpty => Width == 0 || Height == 0;

    public RectangleF Intersect(RectangleF rectangle)
    {
        var x = MathF.Max(X, rectangle.X);
        var y = MathF.Max(Y, rectangle.Y);

        var r = MathF.Min(Right, rectangle.Right);
        var b = MathF.Min(Bottom, rectangle.Bottom);

        var w = MathF.Max(0, r - x);
        var h = MathF.Max(0, b - y);

        return new(x, y, w, h);
    }

    public bool IsIntersecting(RectangleF other)
    {
        if (other.Right <= X) return false;
        if (other.X >= Right) return false;

        if (other.Bottom <= Y) return false;
        if (other.Y >= Bottom) return false;
        
        return true;
    }

    public RectangleF Inflate(float horz, float vert) => Inflate(horz, vert, horz, vert);
    
    public RectangleF Inflate(float all) => Inflate(all, all, all, all);

    public RectangleF Inflate(float left, float top, float right, float bottom)
    {
        var w = Width + left + right;
        var h = Height + top + bottom;

        var x = X - left;
        var y = Y - top;

        return new(x, y, w, h);
    }

    public bool Contains(Vector2 pos)
    {
        if (pos.X < X) return false;
        if (pos.X >= Right) return false;

        if (pos.Y < Y) return false;
        if (pos.Y >= Bottom) return false;
        return true;
    }

    public static implicit operator RectangleF(Rectangle rect) =>
        new(rect.X, rect.Y, rect.Width, rect.Height);
}