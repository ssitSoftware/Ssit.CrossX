using static bottlenoselabs.Interop.SDL;
using Ssit.CrossX.Input;
using Ssit.CrossX.SDL3.Common;

namespace Ssit.CrossX.SDL3.Input;

internal unsafe class SdlGameController: IDisposable
{
    private readonly int _playerIndex;
    private SdlHandle<SDL_Gamepad> _handle = null;
    
    private static readonly HashSet<uint> AttachedControllers = new();
    private readonly HashSet<GameControllerButton> _previousButtons = new();

    public SdlGameController(int playerIndex)
    {
        _playerIndex = playerIndex;
        SDL_FindController();
    }

    public bool IsConnected => SDL_GetGamepadFromPlayerIndex(_playerIndex) != null;

    public float GetAxis(GameControllerAxis axis)
    {
        var gamepad = _handle != null ? _handle.Pointer : null;

        if (gamepad is null)
        {
            return 0;
        }
        
        float value = 0;
        
        if (axis == GameControllerAxis.DPadX)
        {
            value += SDL_GetGamepadButton(gamepad,  SDL_GamepadButton.SDL_GAMEPAD_BUTTON_DPAD_LEFT).Value == 1 ? -1 : 0;
            value += SDL_GetGamepadButton(gamepad,  SDL_GamepadButton.SDL_GAMEPAD_BUTTON_DPAD_RIGHT).Value == 1 ? 1 : 0;
            return value;
        }
        
        if (axis == GameControllerAxis.DPadY)
        {
            value += SDL_GetGamepadButton(gamepad,  SDL_GamepadButton.SDL_GAMEPAD_BUTTON_DPAD_UP).Value == 1 ? -1 : 0;
            value += SDL_GetGamepadButton(gamepad,  SDL_GamepadButton.SDL_GAMEPAD_BUTTON_DPAD_DOWN).Value == 1 ? 1 : 0;
            return value;
        }
        
        value = SDL_GetGamepadAxis(gamepad, (SDL_GamepadAxis)axis);
        return value / short.MaxValue;
    }

    private bool IsButtonDown(GameControllerButton button)
    {
        if (button < GameControllerButton.BasicMax)
        {
            var gamepad = _handle != null ? _handle.Pointer : null;

            if (gamepad is null)
            {
                return false;
            }
            
            return SDL_GetGamepadButton(gamepad, (SDL_GamepadButton)button).Value == 1;
        }

        switch (button)
        {
            case GameControllerButton.LeftStickLeft:
                return GetAxis(GameControllerAxis.LeftX) < -0.5f;
            
            case GameControllerButton.LeftStickRight:
                return GetAxis(GameControllerAxis.LeftX) > 0.5f;
            
            case GameControllerButton.LeftStickUp:
                return GetAxis(GameControllerAxis.LeftY) < -0.5f;
            
            case GameControllerButton.LeftStickDown:
                return GetAxis(GameControllerAxis.LeftY) > 0.5f;
            
            case GameControllerButton.RightStickLeft:
                return GetAxis(GameControllerAxis.RightX) < -0.5f;
            
            case GameControllerButton.RightStickRight:
                return GetAxis(GameControllerAxis.RightX) > 0.5f;
            
            case GameControllerButton.RightStickUp:
                return GetAxis(GameControllerAxis.RightY) < -0.5f;
            
            case GameControllerButton.RightStickDown:
                return GetAxis(GameControllerAxis.RightY) > 0.5f;
            
            case GameControllerButton.LeftTrigger:
                return GetAxis(GameControllerAxis.LeftTrigger) > 0.5f;
            
            case GameControllerButton.RightTrigger:
                return GetAxis(GameControllerAxis.RightTrigger) > 0.5f;
        }
        
        return false;
    }
    
    public ButtonState GetButton(GameControllerButton button)
    {
        bool isDown = IsButtonDown(button);
        bool wasDown = _previousButtons.Contains(button);

        return new ButtonState(isDown, isDown != wasDown);
    }
    
    public void PostUpdate()
    {
        _previousButtons.Clear();
        for (var idx = 0; idx < (int) GameControllerButton.Max; ++idx)
        {
            if (IsButtonDown((GameControllerButton) idx))
            {
                _previousButtons.Add((GameControllerButton)idx);
            }
        }
    }

    private void CloseHandle()
    {
        var gamepad = _handle != null ? _handle.Pointer : null;
        if (gamepad != null)
        {
            SDL_CloseGamepad(gamepad);
            _handle = null;
        }
    }
    
    public void Dispose() => CloseHandle();

    public void ProcessEvent(SDL_Event sdlEvent)
    {
        switch ((SDL_EventType)sdlEvent.type)
        {
            case SDL_EventType.SDL_EVENT_GAMEPAD_ADDED:
                if (_handle == null || _handle.Pointer == null)
                {
                    SDL_FindController();
                }
                break;
            
            case SDL_EventType.SDL_EVENT_GAMEPAD_REMOVED:

                if (_handle != null && _handle.Pointer != null)
                {
                    if (SDL_GetGamepadConnectionState(_handle.Pointer) <= 0)
                    {
                        CloseHandle();
                    }
                }
                break;
        }
    }
    
    private void SDL_FindController()
    {
        int count;
        var joysticks = SDL_GetJoysticks(&count);

        try
        {
            for (int i = 0; i < count; i++)
            {
                if (SDL_IsGamepad(joysticks[i]).Value == 1)
                {
                    if (!AttachedControllers.Add(joysticks[i]))
                        continue;

                    var gp = SDL_OpenGamepad(joysticks[i]);
                    SDL_SetGamepadPlayerIndex(gp, _playerIndex);
                    _handle = new SdlHandle<SDL_Gamepad>(gp);
                    break;
                }
            }
        }
        finally
        {
            SDL_free(joysticks);
        }
    }
}