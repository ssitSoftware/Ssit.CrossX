using System;

namespace Ssit.CrossX.Input;

public interface INativeTextInput: IDisposable
{
    void UpdatePosition(RectangleF bounds, int cursorPosition);
}