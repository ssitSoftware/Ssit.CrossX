using SkiaSharp;

namespace Ssit.CrossX.SDL.Graphics;

public static class Extensions
{
    public static float DistanceTo(this SKColor color, RgbaColor other)
    {
        var dr = Math.Abs(color.Red - other.R);
        var dg = Math.Abs(color.Green - other.G);
        var db = Math.Abs(color.Blue - other.B);

        return MathF.Sqrt(dr * dr + dg * dg + db * db);
    }
}