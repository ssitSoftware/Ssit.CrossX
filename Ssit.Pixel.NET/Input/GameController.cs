using System;
using System.Collections.Generic;
using Ssit.Pixel.Core;
using Ssit.Pixel.Input;
using static SDL2.Bindings.SDL;

namespace Ssit.Pixel.NET.Input;

internal class GameController: IDisposable
{
    private IntPtr _handle = SDL_FindController();

    private static readonly HashSet<int> AttachedControllers = new();
    
    private readonly HashSet<GameControllerButton> _previousButtons = new();
    
    public void ProcessEvent(SDL_Event @event, IActionScheduler actionScheduler)
    {
        switch (@event.type)
        {
            case SDL_EventType.SDL_CONTROLLERDEVICEADDED:
                if (IntPtr.Zero == _handle)
                {
                    actionScheduler.Schedule(() => _handle = SDL_FindController());
                }
                break;

            case SDL_EventType.SDL_CONTROLLERDEVICEREMOVED:
                if (_handle != IntPtr.Zero && 
                    @event.cdevice.which == SDL_JoystickInstanceID(SDL_GameControllerGetJoystick(_handle)))
                {
                    actionScheduler.Schedule(() =>
                    {
                        AttachedControllers.Remove(@event.cdevice.which);
                        SDL_GameControllerClose(_handle);
                        _handle = SDL_FindController();
                    });
                }
                break;
        }
    }
    
    private static IntPtr SDL_FindController()
    {
        for (int i = 0; i < SDL_NumJoysticks(); i++) 
        {
            if (SDL_IsGameController(i) == SDL_bool.SDL_TRUE)
            {
                if (AttachedControllers.Contains(i))
                    continue;
                
                AttachedControllers.Add(i);
                return SDL_GameControllerOpen(i);
            }
        }
        return IntPtr.Zero;
    }

    public float GetAxis(GameControllerAxis axis)
    {
        if (_handle == IntPtr.Zero)
            return 0f;

        float value = 0;
        
        if (axis == GameControllerAxis.DPadX)
        {
            value += SDL_GameControllerGetButton(_handle, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_DPAD_LEFT) == 1 ? -1 : 0;
            value += SDL_GameControllerGetButton(_handle, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_DPAD_RIGHT) == 1 ? 1 : 0;
            return value;
        }
        
        if (axis == GameControllerAxis.DPadY)
        {
            value += SDL_GameControllerGetButton(_handle, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_DPAD_UP) == 1 ? -1 : 0;
            value += SDL_GameControllerGetButton(_handle, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_DPAD_DOWN) == 1 ? 1 : 0;
            return value;
        }
        
        value = SDL_GameControllerGetAxis(_handle, (SDL_GameControllerAxis)axis);
        return value / short.MaxValue;
    }

    private bool IsButtonDown(GameControllerButton button)
    {
        if (button < GameControllerButton.BasicMax)
        {
            return SDL_GameControllerGetButton(_handle, (SDL_GameControllerButton) button) == 1;
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
        if (_handle == IntPtr.Zero)
            return ButtonState.Empty;

        bool isDown = IsButtonDown(button);
        bool wasDown = _previousButtons.Contains(button);

        return new ButtonState(isDown, isDown != wasDown);
    }
    
    public void PostUpdate()
    {
        _previousButtons.Clear();
        
        if (_handle == IntPtr.Zero) return;
        
        for (var idx = 0; idx < (int) GameControllerButton.Max; ++idx)
        {
            if (IsButtonDown((GameControllerButton) idx))
            {
                _previousButtons.Add((GameControllerButton)idx);
            }
        }
    }
    
    public void Vibrate(ushort low, ushort high, uint ms)
    {
        if (_handle != IntPtr.Zero)
        {
            SDL_GameControllerRumble(_handle, low, high, ms);
        }
    }

    public void Dispose()
    {
        if (_handle == IntPtr.Zero)
            return;
        
        SDL_GameControllerClose(_handle);
        _handle = IntPtr.Zero;
    }
}