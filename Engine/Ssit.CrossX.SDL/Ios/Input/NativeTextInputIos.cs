#if IOS
using Ssit.CrossX.Input;

namespace Ssit.CrossX.SDL.Ios.Input;

internal class NativeTextInputIos(NativeTextInputServiceIos service) : INativeTextInput
{
    private bool _disposed;

    public bool IsShiftPressed => service.IsShiftPressed;

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        service.OnDisposed(this);
    }

    public void UpdatePosition(RectangleF bounds, int cursorPosition)
    {
        if (!_disposed)
            service.UpdatePosition(bounds, cursorPosition);
    }
}
#endif
