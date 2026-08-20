using System;
using System.Numerics;

namespace Ssit.CrossX.XxGames.Utils;

public static class MathUtils
{
    public static Vector2 TrimVectorToPixels(this Vector2 vec, float pixelsInOne)
    {
        if (pixelsInOne <= float.Epsilon)
            return vec;
        
        return new((int)(vec.X * pixelsInOne) / pixelsInOne, (int)(vec.Y * pixelsInOne) / pixelsInOne);
    }
}