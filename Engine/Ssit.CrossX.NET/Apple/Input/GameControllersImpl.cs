#if __MACCATALYST__ || __IOS__

using GameController;
using Ssit.CrossX.Core;
using Ssit.CrossX.Input;
using Ssit.CrossX.Input.Internal;
using Ssit.CrossX.IoC;

namespace Ssit.CrossX.NET.Apple.Input;

internal class GameControllersImpl: GameControllersBase
{
    private const float CheckControllersPeriod = 2;
    
    private readonly GameControllerImpl[] _gameControllers = new GameControllerImpl[4];
    private readonly GameControllerImpl[] _tempBuffer = new GameControllerImpl[4];
    
    private float _nextCheck = 1;
    
    public GameControllersImpl(IEventSource eventSource, IIoCContainer container)
    {
        DiscoverControllers();

        eventSource.Updating += Updating;
        eventSource.Updated += PostUpdate;
    }

    private void Updating(float dt)
    {
        _nextCheck -= dt;

        if (_nextCheck <= 0)
        {
            _nextCheck = CheckControllersPeriod;
            DiscoverControllers();
        }
    }

    private void DiscoverControllers()
    {
        for (var idx = 0; idx < _tempBuffer.Length; ++idx)
        {
            _tempBuffer[idx] = null;
        }
        
        var controllers = GCController.Controllers;

        foreach (var controller in controllers)
        {
            if (controller.PlayerIndex == GCControllerPlayerIndex.Unset)
            {
                for (var idx = 0; idx < _gameControllers.Length; ++idx)
                {
                    if (_gameControllers[idx] == null)
                    {
                        controller.PlayerIndex = (GCControllerPlayerIndex) idx;
                        break;
                    }
                }
            }
            
            var index = (int) controller.PlayerIndex;

            if (index >= 0 && index < 4)
            {
                if (_gameControllers[index] == null)
                {
                    _tempBuffer[index] = new GameControllerImpl(controller);
                }
                else if(ReferenceEquals(_gameControllers[index].Controller, controller))
                {
                    _tempBuffer[index] = _gameControllers[index];
                }
                else
                {
                    _tempBuffer[index] = new GameControllerImpl(controller);
                }
            }
        }
        
        for (var idx = 0; idx < _tempBuffer.Length; ++idx)
        {
            _gameControllers[idx] = _tempBuffer[idx];
        }
    }

    protected override bool IsConnectedInternal(int player)
    {
        return _gameControllers[player] != null;
    }

    protected override ButtonState GetButtonInternal(int player, GameControllerButton button)
    {
        return _gameControllers[player]?.GetButton(button) ?? ButtonState.Empty;
    }

    protected override float GetAxisInternal(int player, GameControllerAxis axis)
    {
        return _gameControllers[player]?.GetAxis(axis) ?? 0f;
    }

    protected override void VibrateInternal(int player, ushort low, ushort high, uint ms)
    {
        _gameControllers[player]?.Vibrate(low, high, ms);
    }

    private void PostUpdate()
    {
        foreach (var controller in _gameControllers)
        {
            controller?.PostUpdate();
        }
    }
}

#endif