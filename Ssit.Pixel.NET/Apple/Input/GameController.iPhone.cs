#if __IOS__ && !__MACCATALYST__

using System;
using Ssit.Pixel.Input;

namespace Ssit.Pixel.NET.Apple.Input;

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

#endif