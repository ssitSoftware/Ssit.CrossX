using System;

namespace Ssit.CrossX.Input.Internal;

public interface IGameController: IDisposable
{
    float GetAxis(GameControllerAxis axis);
    ButtonState GetButton(GameControllerButton button);
    void PostUpdate();
    void Vibrate(ushort low, ushort high, uint ms);
}