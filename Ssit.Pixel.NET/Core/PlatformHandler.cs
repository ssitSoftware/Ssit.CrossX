using System;
using System.Diagnostics;
using Ssit.Pixel.Core;
using Ssit.Pixel.Input;
using Ssit.Pixel.IoC;
using Ssit.Pixel.NET.Input;
using static SDL2.Bindings.SDL;

namespace Ssit.Pixel.NET.Core;

internal class PlatformHandler
{
    private GameControllersImpl _gameControllers;
    
    private readonly Stopwatch _stopwatch = new();
    private TimeSpan _lastTime;
        
    public PlatformHandler()
    {
        _stopwatch.Start();
        _lastTime = _stopwatch.Elapsed;
    }
    
    public void Tick(IApp app)
    {
        while (SDL_PollEvent(out SDL_Event e) == 1)
        {
            _gameControllers.ProcessEvent(e);
        }
        
        var stopwatchRead = _stopwatch.Elapsed;
        var dt = (float)(stopwatchRead - _lastTime).TotalSeconds;
        _lastTime = stopwatchRead;
        
        app.Update(dt);
        _gameControllers.PostUpdate();
    }

    public void Initialize(IIoCContainerBuilder builder)
    {
        SDL_Init(SDL_INIT_EVENTS | SDL_INIT_GAMECONTROLLER | SDL_INIT_AUDIO);
        
        _gameControllers = new GameControllersImpl();
        builder.WithInstance<IGameControllers>(_gameControllers);
    }
}