using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Numerics;

namespace Ssit.CrossX.XxGames.Physics;

public struct Aabb
{
    public static readonly Aabb Empty = new();

    public float Top;
    public float Left;
    public float Right;
    public float Bottom;

    public float Width => Right - Left;
    public float Height => Bottom - Top;

    public Vector2 Center => new( (Left+Right) / 2f, (Top+Bottom) / 2f);

    public Aabb(float left, float top, float right, float bottom)
    {
        Top = top;
        Bottom = bottom;
        Left = left;
        Right = right;
    }

    public Aabb(PointF center, SizeF size)
    {
        Top = center.Y - size.Height / 2;
        Bottom = center.Y + size.Height / 2;
        Left = center.X - size.Width / 2;
        Right = center.X + size.Width / 2;
    }

    [Pure]
    public bool Intersects(Aabb other)
    {
        if (other.Left > Right) return false;
        if (other.Right < Left) return false;
        if (other.Top > Bottom) return false;
        if (other.Bottom < Top) return false;
        return true;
    }

    [Pure]
    public bool Intersects(Aabb other, double epsilon)
    {
        if (other.Left + epsilon > Right) return false;
        if (other.Right - epsilon < Left) return false;
        if (other.Top + epsilon > Bottom) return false;
        if (other.Bottom - epsilon < Top) return false;
        return true;
    }

    public void Offset(Vector2 offset)
    {
        Top += offset.Y;
        Bottom += offset.Y;
        Left += offset.X;
        Right += offset.X;
    }

    [Pure]
    public Aabb GetOffset(Vector2 offset)
    {
        return new Aabb
        {
            Top = Top + offset.Y,
            Bottom = Bottom + offset.Y,
            Left = Left + offset.X,
            Right = Right + offset.X
        };
    }

    [Pure]
    public bool Contains(PointF point)
    {
        return point.X >= Left && point.X <= Right && point.Y >= Top && point.Y <= Bottom;
    }

    [Pure]
    public bool Contains(Aabb other)
    {
        if (other.Left < Left) return false;
        if (other.Right > Right) return false;
        if (other.Top < Top) return false;
        if (other.Bottom > Bottom) return false;
        return true;
    }

    [Pure]
    public Aabb Union(Aabb aabb)
    {
        return new Aabb
        {
            Left = Math.Min(Left, aabb.Left),
            Top = Math.Min(Top, aabb.Top),
            Right = Math.Max(Right, aabb.Right),
            Bottom = Math.Max(Bottom, aabb.Bottom),
        };
    }

    public override string ToString()
    {
        return $"Aabb ({Left}, {Top}, {Right}, {Bottom})";
    }

    public void Inflate(float size)
    {
        Left -= size;
        Right += size;
        Top -= size;
        Bottom += size;
    }

    public static Aabb operator *(Aabb aabb, float multiply)
    {
        return new Aabb(aabb.Left * multiply, aabb.Top * multiply, aabb.Right * multiply, aabb.Bottom * multiply);
    }

    public static explicit operator RectangleF(Aabb aabb)
    {
        return new RectangleF(aabb.Left, aabb.Top, aabb.Right-aabb.Left, aabb.Bottom-aabb.Top);
    }
}