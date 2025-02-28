using System.Numerics;
using Ssit.CrossX.Input;

namespace Ssit.CrossX.SDL.Input;

public class SdlPointingDevices: IPointingDevices
{
    public bool Enable { get; set; }
    public IReadOnlyList<Pointer> Pointers => [];
    
    public Pointer GetPointer(int id)
    {
        return null;
    }

    public Vector2? HoverPosition => null;

    public bool ShowHoverPointer { get; set; }
    public void PushTransform(Matrix3x2 transform)
    {
    }

    public void PopTransform()
    {
    }

    public void SetHoverPosition(Vector2 position)
    {
    }

    public float CalculateHorizontalVelocity(int id)
    {
        return 0;
    }

    public float CalculateVerticalVelocity(int id)
    {
        return 0;
    }
}