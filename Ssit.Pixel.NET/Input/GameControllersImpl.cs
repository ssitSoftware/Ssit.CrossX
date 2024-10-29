using System;
using Ssit.Pixel.Input;
using Ssit.Pixel.Input.Internal;
using Ssit.Pixel.NET.Core;

namespace Ssit.Pixel.NET.Input;

internal class GameControllersImpl: GameControllersBase
{
    private readonly GameController[] _gameControllers = new GameController[4];

    public GameControllersImpl()
    {
        for (var idx = 0; idx < _gameControllers.Length; ++idx)
        {
            _gameControllers[idx] = new GameController();
        }
    }
    
    protected override ButtonState GetButtonInternal(int player, GameControllerButton button)
    {
        CheckPlayerIndex(player);
        return _gameControllers[player].GetButton(button);
    }

    protected override float GetAxisInternal(int player, GameControllerAxis axis)
    {
        CheckPlayerIndex(player);
        return _gameControllers[player].GetAxis(axis);
    }

    protected override void VibrateInternal(int player, ushort low, ushort high, uint ms)
    {
        CheckPlayerIndex(player);
        _gameControllers[player].Vibrate(low, high, ms);
    }
    
    public void PostUpdate()
    {
        foreach (var controller in _gameControllers)
        {
            controller.PostUpdate();
        }
    }

    public void ProcessEvent(SDL2.Bindings.SDL.SDL_Event e, IActionScheduler actionScheduler)
    {
        foreach (var controller in _gameControllers)
        {
            controller.ProcessEvent(e, actionScheduler);
        }
    }
    
    private void CheckPlayerIndex(int player)
    {
        if (player < 0 || player >= _gameControllers.Length) 
            throw new ArgumentException($"There can only be {_gameControllers.Length} players! Acceptable values are 0..{_gameControllers.Length-1}.", nameof(player));
    }
}