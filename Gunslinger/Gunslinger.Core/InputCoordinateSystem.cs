using System.Numerics;
using Ssit.CrossX.Core;
using Ssit.CrossX.UI.Services;

namespace Gunslinger.Core;

public class InputCoordinateSystem(PixelAppHost appHost): IInputCoordinateSystem
{
    public Matrix3x2 Transform
    {
        get
        {
            if (Matrix3x2.Invert(appHost.Transform, out var matrix))
            {
                return matrix;
            }

            return Matrix3x2.Identity;
        }
    }
}