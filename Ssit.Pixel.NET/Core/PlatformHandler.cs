using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Ssit.Pixel.Core;
using Ssit.Pixel.Input;
using Ssit.Pixel.IoC;
using Ssit.Pixel.NET.Input;
using static SDL2.Bindings.SDL;

namespace Ssit.Pixel.NET.Core;

internal class PlatformHandler: IActionScheduler
{
    private GameControllersImpl _gameControllers;
    
    private readonly Stopwatch _stopwatch = new();
    private TimeSpan _lastTime;

    private Task _sdlEventsTask;
    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    
    private ConcurrentQueue<Action> _actionQueue = new();
    
    public PlatformHandler()
    {
        _stopwatch.Start();
        _lastTime = _stopwatch.Elapsed;

        _sdlEventsTask = Task.Run(() => RunSdlEventsTask(_cancellationTokenSource.Token));
    }

    private async Task RunSdlEventsTask(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            while (SDL_PollEvent(out SDL_Event e) == 1)
            {
                _gameControllers.ProcessEvent(e, this);
            }

            await Task.Delay(10, token);
        }
    }
    
    public void Tick(IApp app)
    {
        while (_actionQueue.TryDequeue(out var action))
        {
            action();
        }
        
        var stopwatchRead = _stopwatch.Elapsed;
        var dt = (float)(stopwatchRead - _lastTime).TotalSeconds;
        _lastTime = stopwatchRead;
        
        app.Update(dt);
        //_gameControllers.PostUpdate();
    }

    public void Initialize(IIoCContainerBuilder builder)
    {
        SDL_Init(SDL_INIT_EVENTS | SDL_INIT_GAMECONTROLLER | SDL_INIT_AUDIO);
        
        _gameControllers = new GameControllersImpl();
        builder.WithInstance<IGameControllers>(_gameControllers);
    }

    public void Schedule(Action action)
    {
        _actionQueue.Enqueue(action);
    }
}