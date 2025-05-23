using System;
using System.Numerics;

namespace Ssit.CrossX.Games.Utils;

public static class MathUtils
{
    public static Vector2 TrimVectorToPixels(this Vector2 vec, float pixelsInOne) => new(MathF.Floor(vec.X * pixelsInOne) / pixelsInOne, MathF.Floor(vec.Y * pixelsInOne) / pixelsInOne);
}