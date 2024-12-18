using System.Collections.Generic;
using System.Numerics;

namespace Ssit.CrossX.Input;

public interface IPointingDevices
{
    IReadOnlyList<Pointer> Pointers { get; }
    Pointer GetPointer(int id);
    Vector2? HoverPosition { get; }

    float CalculateHorizontalVelocity(int id);
    float CalculateVerticalVelocity(int id);
}