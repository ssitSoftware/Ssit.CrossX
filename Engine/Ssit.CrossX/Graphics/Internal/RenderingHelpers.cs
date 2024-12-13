using System.Numerics;

namespace Ssit.CrossX.Graphics.Internal;

public static class RenderingHelpers
{
    public static Matrix3x2 CreateTransform(RectangleF rect, ImageTransform transform)
    {
        return Matrix3x2.Identity;
    }
    
    public static Vector3 Prepare(this Vector3 vector, ref Matrix3x2 matrix)
    {
        return new Vector3(Vector2.Transform(new Vector2(vector.X, vector.Y), matrix), vector.Z);
    }
}