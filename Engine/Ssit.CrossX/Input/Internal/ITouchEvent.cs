using System.Numerics;

namespace Ssit.CrossX.Input.Internal;

public interface ITouchEvent
{
    int Id { get; }
    Vector2 Origin { get; }
    Vector2 Position { get; }
    void Capture(object context);
    object CapturedBy { get; }
}