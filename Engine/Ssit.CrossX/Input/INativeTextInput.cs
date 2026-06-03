using System;

namespace Ssit.CrossX.Input;

public interface INativeTextInput: IDisposable
{
    bool IsShiftPressed { get; }
    void UpdatePosition(RectangleF bounds, int cursorPosition);
    void Reactivate();
}