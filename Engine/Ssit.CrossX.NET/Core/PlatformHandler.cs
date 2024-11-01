using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Ssit.CrossX.Audio;
using Ssit.CrossX.Audio.Internal;
using Ssit.CrossX.Core;
using Ssit.CrossX.Input;
using Ssit.CrossX.IoC;
using Ssit.CrossX.NET.Audio;
using Ssit.CrossX.NET.Input;

#if __MACCATALYST__
    using static SDL2.Bindings.SDL;
#endif

namespace Ssit.CrossX.NET.Core;

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
#if __MACCATALYST__
        while (!token.IsCancellationRequested)
        {
            while (SDL_PollEvent(out SDL_Event e) == 1)
            {
                _gameControllers.ProcessEvent(e, this);
            }

            await Task.Delay(10, token);
        }
#endif
    }
    
    public void Tick(IApp app, Action<float> preUpdate)
    {
        var stopwatchRead = _stopwatch.Elapsed;
                var dt = (float)(stopwatchRead - _lastTime).TotalSeconds;
                _lastTime = stopwatchRead;

        preUpdate(dt);
                
        while (_actionQueue.TryDequeue(out var action))
        {
            action();
        }
        
        app.Update(dt);
        _gameControllers.PostUpdate();
    }

    public void Initialize(IIoCContainerBuilder builder)
    {
    #if __MACCATALYST__
        SDL_Init(SDL_INIT_EVENTS | SDL_INIT_GAMECONTROLLER);
    #endif
        
        _gameControllers = new GameControllersImpl();
        var soundManager = new SoundManagerImpl();
        
        builder
            .WithInstance<IGameControllers>(_gameControllers)
            .WithInstance<ISoundManager>(soundManager)
            .WithImplementation<ISingleMusicPlayer, SingleMusicPlayerImpl>()
            .WithInstance<IActionScheduler>(this);
    }

    public void Schedule(Action action)
    {
        _actionQueue.Enqueue(action);
    }
}