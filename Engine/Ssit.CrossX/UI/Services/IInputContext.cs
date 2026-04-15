using Ssit.CrossX.UI.Handlers;

namespace Ssit.CrossX.UI.Services;

public interface IInputContext
{
    void CapturePointer(int pointerId, IInputConsumer captureBy);
    bool Focus(IFocusable focusable, object caller);
    IFocusable FindFocusable(string uniqueId, object caller);
    bool MoveFocus(FocusDirection direction, object caller);
}