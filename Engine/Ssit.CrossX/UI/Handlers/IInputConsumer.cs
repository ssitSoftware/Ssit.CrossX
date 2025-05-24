using System.Collections.Generic;
using System.Numerics;
using Ssit.CrossX.Input;
using Ssit.CrossX.UI.Services;

namespace Ssit.CrossX.UI.Handlers;

public interface IInputConsumer
{
    void ProcessHover(Vector2? hoverPosition, int? matchingPointerId, IInputContext context);
    bool ProcessInput(IReadOnlyList<Pointer> pointer, IInputContext context);
    void CancelPointer(int pointerId, IInputContext context);
}