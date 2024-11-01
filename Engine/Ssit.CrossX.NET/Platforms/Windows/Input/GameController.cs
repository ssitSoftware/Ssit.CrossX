using System;
using Ssit.CrossX.Input;

namespace Ssit.CrossX.NET.Input;

internal class GameController: IDisposable
{
    public float GetAxis(GameControllerAxis axis)
    {
        return 0;
    }

    private bool IsButtonDown(GameControllerButton button)
    {
        return false;
    }
    
    public ButtonState GetButton(GameControllerButton button)
    {
        return ButtonState.Empty;
    }
    
    public void PostUpdate()
    {
    }
    
    public void Vibrate(ushort low, ushort high, uint ms)
    {
    }

    public void Dispose()
    {
    }
}