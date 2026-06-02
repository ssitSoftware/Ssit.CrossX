using System.Numerics;
using Ssit.CrossX.Core;

namespace Ssit.CrossX.UI.Services;

internal class InputCoordinateSystem(IAppHost appHost): IInputCoordinateSystem
{
    public Matrix3x2 Transform => appHost.TransformInv;
    public Matrix3x2 TransformInv => appHost.Transform;
}