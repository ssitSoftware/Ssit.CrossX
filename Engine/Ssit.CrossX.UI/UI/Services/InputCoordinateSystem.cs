using System.Numerics;
using Ssit.CrossX.Core;

namespace Ssit.CrossX.UI.Services;

internal class InputCoordinateSystem(IAppHost appHost): IInputCoordinateSystem
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