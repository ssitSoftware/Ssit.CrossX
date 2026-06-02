#if ANDROID
using Ssit.CrossX.Input;

namespace Ssit.CrossX.SDL.Droid.Input;

internal class NativeTextInputDroid(NativeTextInputServiceDroid service) : INativeTextInput
{
    private bool _disposed;

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        service.OnDisposed(this);
    }

    public void UpdatePosition(RectangleF bounds, int cursorPosition)
    {
        if (!_disposed)
        {
            service.UpdatePosition(bounds, cursorPosition);
        }
    }
}
#endif
