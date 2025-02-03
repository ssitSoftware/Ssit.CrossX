using System.Collections.Generic;
using System.Numerics;

namespace Ssit.CrossX.Input;

public interface IPointingDevices
{
    bool Enable { get; set; }
    IReadOnlyList<Pointer> Pointers { get; }
    Pointer GetPointer(int id);
    Vector2? HoverPosition { get; }
    
    bool ShowHoverPointer { get; set; }
    
    void PushTransform(Matrix3x2 transform);
    void PopTransform();
    
    void SetHoverPosition(Vector2 position);
    float CalculateHorizontalVelocity(int id);
    float CalculateVerticalVelocity(int id);
}