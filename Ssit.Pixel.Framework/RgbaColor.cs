using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Ssit.Pixel.Framework;

/// <summary>
/// Represents a color in the RGBA (Red, Green, Blue, Alpha) color space.
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 1)]
[DebuggerDisplay("RgbaColor = ({R}, {G}, {B}, {A})")]
public partial struct RgbaColor : IEquatable<RgbaColor>
{
    public RgbaColor(long color)
    {
        R = (byte)(color & 0xff);
        G = (byte)((color >> 8) & 0xff);
        B = (byte)((color >> 16) & 0xff);
        A = (byte)((color >> 24) & 0xff);
    }

    public RgbaColor(byte red, byte green, byte blue, byte alpha = 255)
    {
        R = red;
        G = green;
        B = blue;
        A = alpha;
    }
    
    public RgbaColor(float red, float green, float blue, float alpha = 1.0f)
    {
        R = (byte)(red * 255);
        G = (byte)(green * 255);
        B = (byte)(blue * 255);
        A = (byte)(alpha * 255);
    }

    public RgbaColor Mix(RgbaColor other, float mix)
    {
        var red = Rf * (1 - mix) + other.Rf * mix;
        var green = Gf * (1 - mix) + other.Gf * mix;
        var blue = Bf * (1 - mix) + other.Bf * mix;
        var alpha = Af * (1 - mix) + other.Af * mix;

        return new(red, green, blue, alpha);
    }

    public int ToInt32()
    {
        long argb = (B | (G << 8) | (R << 16) | (A << 24));
        return (int)argb;
    }

    public uint ToUInt32()
    {
        long argb = (R | (G << 8) | (B << 16) | (A << 24));
        return (uint)argb;
    }
    
    public uint ToUInt32Inverted()
    {
        long argb = (A | (B << 8) | (G << 16) | (R << 24));
        return (uint)argb;
    }
    
    public byte R;
    public byte G;
    public byte B;
    public byte A;

    public float Rf => R / 255.0f;
    public float Gf => G / 255.0f;
    public float Bf => B / 255.0f;
    public float Af => A / 255.0f;

    public static RgbaColor operator * (RgbaColor color1, RgbaColor color2)
    {
        return new(color1.Rf * color2.Rf, color1.Gf * color2.Gf, color1.Bf * color2.Bf, color1.Af * color2.Af);
    }

    public static RgbaColor operator *(RgbaColor color, float multiply) => new(color.Rf, color.Gf, color.Bf, color.Af * multiply);
    public static RgbaColor operator *(RgbaColor color, double multiply) => color * (float)multiply;

    public bool Equals(RgbaColor other) => R == other.R && G == other.G && B == other.B && A == other.A;

    public static bool operator ==(RgbaColor c1, RgbaColor c2) => c1.Equals(c2);

    public static bool operator !=(RgbaColor c1, RgbaColor c2) => !c1.Equals(c2);

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        return obj is RgbaColor && Equals((RgbaColor)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = R.GetHashCode();
            hashCode = (hashCode * 397) ^ G.GetHashCode();
            hashCode = (hashCode * 397) ^ B.GetHashCode();
            hashCode = (hashCode * 397) ^ A.GetHashCode();
            return hashCode;
        }
    }

    public static RgbaColor FromInt32(int intColor)
    {
        var a = (intColor >> 24) & 0xff;
        var r = (intColor >> 16) & 0xff;
        var g = (intColor >> 8) & 0xff;
        var b = intColor & 0xff;

        return new((byte)r, (byte)g, (byte)b, (byte)a);
    }
    
    public static RgbaColor FromUInt32(uint intColor)
    {
        var a = (intColor >> 24) & 0xff;
        var b = (intColor >> 16) & 0xff;
        var g = (intColor >> 8) & 0xff;
        var r = intColor & 0xff;

        return new((byte)r, (byte)g, (byte)b, (byte)a);
    }

    public readonly RgbaColor WithAlpha(byte alpha)
    {
        return new(R, G, B, alpha);
    }

    public readonly Color ToDrawingColor() => Color.FromArgb(A, R, G, B);
    public static implicit operator RgbaColor(Color color) => new RgbaColor(color.R, color.G, color.B, color.A);
}