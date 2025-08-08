using SDL;
using Ssit.CrossX.Input;

namespace Ssit.CrossX.SDL.Input;

internal class SdlGameControllers: IGameControllers
{
    public byte VibrationForce { get; set; }

    private readonly SdlGameController[] _controllers =
    [
        new(0),
        new(1),
        new(2),
        new(3)
    ];
    
    public ButtonState GetButton(int player, GameControllerButton button)
    {
        return _controllers[player].GetButton(button);
    }

    public float GetAxis(int player, GameControllerAxis axis)
    {
        return _controllers[player].GetAxis(axis);
    }

    public bool IsConnected(int player)
    {
        return _controllers[player].IsConnected;
    }
    
    public void PostUpdate()
    {
        foreach (var controller in _controllers)
        {
            controller.PostUpdate();
        }
    }

    public void ProcessEvent(SDL_Event e)
    {
        foreach (var controller in _controllers)
        {
            controller.ProcessEvent(e);
        }
    }
}