using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Ssit.CrossX;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct RgbaColorF
{
    /// <summary>
    /// Represents the red component of the color in the RGBA color space.
    /// </summary>
    public float R;

    /// <summary>
    /// Represents the green component of the color in the RGBA color space.
    /// </summary>
    public float G;

    /// <summary>
    /// Represents the blue component of the color in the RGBA color space.
    /// </summary>
    public float B;

    /// <summary>
    /// Represents the alpha component of the RGBA color,
    /// specifying the transparency level. A value of 255
    /// represents full opacity, while a value of 0 represents
    /// full transparency.
    /// </summary>
    public float A;
    
    public static implicit operator RgbaColorF(RgbaColor color) => new ()
    {
        R = color.Rf,
        G = color.Gf,
        B = color.Bf,
        A = color.Af
    };
}

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

    /// <summary>
    /// Blends this color with another color based on a specified mix ratio.
    /// </summary>
    /// <param name="other">The other RgbaColor to blend with.</param>
    /// <param name="mix">The mix ratio, where 0 results in the original color and 1 results in the other color.</param>
    /// <returns>A new RgbaColor that is the result of the blend operation.</returns>
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

    /// <summary>
    /// Represents the red component of the color in the RGBA color space.
    /// </summary>
    public byte R;

    /// <summary>
    /// Represents the green component of the color in the RGBA color space.
    /// </summary>
    public byte G;

    /// <summary>
    /// Represents the blue component of the color in the RGBA color space.
    /// </summary>
    public byte B;

    /// <summary>
    /// Represents the alpha component of the RGBA color,
    /// specifying the transparency level. A value of 255
    /// represents full opacity, while a value of 0 represents
    /// full transparency.
    /// </summary>
    public byte A;

    /// <summary>
    /// Represents the normalized red component of the color in the RGBA color space.
    /// The value is in the range of 0.0 to 1.0.
    /// </summary>
    public float Rf => R / 255.0f;

    /// <summary>
    /// Represents the normalized green component of the color in the RGBA color space.
    /// The value is in the range of 0.0 to 1.0.
    /// </summary>
    public float Gf => G / 255.0f;

    /// <summary>
    /// Represents the normalized blue component of the color in the RGBA color space.
    /// The value is in the range of 0.0 to 1.0.
    /// </summary>
    public float Bf => B / 255.0f;

    /// <summary>
    /// Represents the normalized alpha component of the color in the RGBA color space.
    /// The value is in the range of 0.0 to 1.0, where 0.0 is full transparency and 1.0 is full opacity.
    /// </summary>
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

    /// <summary>
    /// Creates an RgbaColor instance from a 32-bit integer representation of a color.
    /// The integer should be in the format 0xAARRGGBB where AA is the alpha component,
    /// RR is the red component, GG is the green component, and BB is the blue component.
    /// </summary>
    /// <param name="intColor">The 32-bit integer representation of the color.</param>
    /// <returns>An RgbaColor instance representing the specified color.</returns>
    public static RgbaColor FromInt32(int intColor)
    {
        var a = (intColor >> 24) & 0xff;
        var r = (intColor >> 16) & 0xff;
        var g = (intColor >> 8) & 0xff;
        var b = intColor & 0xff;

        return new((byte)r, (byte)g, (byte)b, (byte)a);
    }

    /// <summary>
    /// Creates an RgbaColor instance from a 32-bit unsigned integer representation of a color.
    /// The integer should be in the format 0xAABBGGRR where AA is the alpha component,
    /// RR is the red component, GG is the green component, and BB is the blue component.
    /// </summary>
    /// <param name="intColor">The 32-bit unsigned integer representation of the color.</param>
    /// <returns>An RgbaColor instance representing the specified color.</returns>
    public static RgbaColor FromUInt32(uint intColor)
    {
        var a = (intColor >> 24) & 0xff;
        var b = (intColor >> 16) & 0xff;
        var g = (intColor >> 8) & 0xff;
        var r = intColor & 0xff;

        return new((byte)r, (byte)g, (byte)b, (byte)a);
    }

    /// <summary>
    /// Returns a new RgbaColor instance with the specified alpha component, retaining the same red, green, and blue components.
    /// </summary>
    /// <param name="alpha">The byte value of the alpha component to be set in the new RgbaColor.</param>
    /// <returns>A new RgbaColor instance with the specified alpha value.</returns>
    public readonly RgbaColor WithAlpha(byte alpha)
    {
        return new(R, G, B, alpha);
    }

    /// <summary>
    /// Converts this RgbaColor instance to a System.Drawing.Color object.
    /// The resulting Color object will have the same red, green, blue, and alpha components
    /// as this RgbaColor instance.
    /// </summary>
    /// <returns>
    /// A System.Drawing.Color object representing the same color as this RgbaColor instance.
    /// </returns>
    public readonly Color ToDrawingColor() => Color.FromArgb(A, R, G, B);

    /// <summary>
    /// Implicitly converts a RgbaColor instance to a System.Drawing.Color instance.
    /// </summary>
    /// <param name="color">The RgbaColor color instance to be converted.</param>
    /// <returns>A System.Drawing.Color instance representing the specified RgbaColor instance.</returns>
    public static implicit operator Color(RgbaColor color) => color.ToDrawingColor();
    
    /// <summary>
    /// Implicitly converts a System.Drawing.Color instance to an RgbaColor instance.
    /// The conversion takes the red, green, blue, and alpha components from the System.Drawing.Color.
    /// </summary>
    /// <param name="color">The System.Drawing.Color instance to be converted.</param>
    /// <returns>An RgbaColor instance representing the specified System.Drawing.Color instance.</returns>
    public static implicit operator RgbaColor(Color color) => new RgbaColor(color.R, color.G, color.B, color.A);
}