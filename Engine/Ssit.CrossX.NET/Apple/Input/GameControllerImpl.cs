#if __MACCATALYST__ || __IOS__

using System.Collections.Generic;
using GameController;
using Ssit.CrossX.Input;
using Ssit.CrossX.Input.Internal;

namespace Ssit.CrossX.NET.Apple.Input;

internal class GameControllerImpl: IGameController
{
    private readonly HashSet<GameControllerButton> _previousButtons = new();
    public GCController Controller { get; }
    
    public GameControllerImpl(GCController controller)
    {
        Controller = controller;
    }

    public float GetAxis(GameControllerAxis axis)
    {
        switch (axis)
        {
            case GameControllerAxis.DPadX:
                return (Controller.ExtendedGamepad?.DPad?.Left.IsPressed ?? false) ? -1 : (Controller.ExtendedGamepad?.DPad?.Right.IsPressed ?? false) ? 1 : 0;
            
            case GameControllerAxis.DPadY:
                return (Controller.ExtendedGamepad?.DPad?.Up.IsPressed ?? false) ? -1 : (Controller.ExtendedGamepad?.DPad?.Down.IsPressed ?? false) ? 1 : 0;
            
            case GameControllerAxis.LeftTrigger:
                return Controller.ExtendedGamepad?.LeftTrigger?.Value ?? 0;
            
            case GameControllerAxis.RightTrigger:
                return Controller.ExtendedGamepad?.RightTrigger?.Value ?? 0;
            
            case GameControllerAxis.LeftX:
                return Controller.ExtendedGamepad?.LeftThumbstick?.XAxis?.Value ?? 0;
            
            case GameControllerAxis.LeftY:
                return -Controller.ExtendedGamepad?.LeftThumbstick?.YAxis?.Value ?? 0;
            
            case GameControllerAxis.RightX:
                return Controller.ExtendedGamepad?.RightThumbstick?.XAxis?.Value ?? 0;
            
            case GameControllerAxis.RightY:
                return -Controller.ExtendedGamepad?.RightThumbstick?.YAxis?.Value ?? 0;
        }

        return 0;
    }

    private bool IsButtonDown(GameControllerButton button)
    {
        if (button < GameControllerButton.BasicMax)
        {
            switch (button)
            {
                case GameControllerButton.A:
                    return Controller.ExtendedGamepad?.ButtonA.IsPressed ?? false;
                
                case GameControllerButton.B:
                    return Controller.ExtendedGamepad?.ButtonB.IsPressed ?? false;
                
                case GameControllerButton.X:
                    return Controller.ExtendedGamepad?.ButtonX.IsPressed ?? false;
                
                case GameControllerButton.Y:
                    return Controller.ExtendedGamepad?.ButtonY.IsPressed ?? false;
                
                case GameControllerButton.Back:
                    return Controller.ExtendedGamepad?.ButtonOptions?.IsPressed ?? false;
                
                case GameControllerButton.Start:
                    return (Controller.ExtendedGamepad?.ButtonMenu?.Touched ?? false) ||
                           (Controller.ExtendedGamepad?.ButtonMenu?.IsPressed ?? false);
                
                case GameControllerButton.Guide:
                    return Controller.ExtendedGamepad?.ButtonHome?.IsPressed ?? false;
                
                case GameControllerButton.LeftShoulder:
                    return Controller.ExtendedGamepad?.LeftShoulder?.IsPressed ?? false;
                
                case GameControllerButton.RightShoulder:
                    return Controller.ExtendedGamepad?.RightShoulder?.IsPressed ?? false;
                
                case GameControllerButton.LeftStick:
                    return Controller.ExtendedGamepad?.LeftThumbstickButton?.IsPressed ?? false;
                
                case GameControllerButton.RightStick:
                    return Controller.ExtendedGamepad?.RightThumbstickButton?.IsPressed ?? false;
                
                case GameControllerButton.DPadLeft:
                    return Controller.ExtendedGamepad?.DPad?.Left?.IsPressed ?? false;
                
                case GameControllerButton.DPadRight:
                    return Controller.ExtendedGamepad?.DPad?.Right?.IsPressed ?? false;
                
                case GameControllerButton.DPadUp:
                    return Controller.ExtendedGamepad?.DPad?.Up?.IsPressed ?? false;
                
                case GameControllerButton.DPadDown:
                    return Controller.ExtendedGamepad?.DPad?.Down?.IsPressed ?? false;
            }
            return false;
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
    
    public void Vibrate(ushort low, ushort high, uint ms)
    {
    }

    public void Dispose()
    {
    }
}

#endif