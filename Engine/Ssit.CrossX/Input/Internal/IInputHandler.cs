using System.Numerics;

namespace Ssit.CrossX.Input.Internal;

public interface IInputHandler
{
    void OnTouch(ulong id, ButtonState state, Vector2? position);
}