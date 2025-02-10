using Ssit.CrossX.Input;

namespace Ssit.CrossX.SDL3.Input;

internal class SdlGameControllers: IGameControllers
{
    public byte VibrationForce { get; set; }
    public ButtonState GetButton(int player, GameControllerButton button)
    {
        return ButtonState.Empty;
    }

    public float GetAxis(int player, GameControllerAxis axis)
    {
        return 0;
    }

    public bool IsConnected(int player)
    {
        return false;
    }
}