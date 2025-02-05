using System.Numerics;

namespace Ssit.CrossX.UI.Services;

public interface IInputCoordinateSystem
{
    Matrix3x2 Transform { get; }
}