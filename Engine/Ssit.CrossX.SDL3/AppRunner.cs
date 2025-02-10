using Interop.Runtime;
using Ssit.CrossX.Core;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.Input;
using Ssit.CrossX.IoC;
using Ssit.CrossX.SDL3.Common;
using Ssit.CrossX.SDL3.Graphics;
using Ssit.CrossX.SDL3.Input;
using Ssit.CrossX.SDL3.Services;
using static bottlenoselabs.Interop.SDL;

namespace Ssit.CrossX.SDL3;

public static class AppRunner<TApp> where TApp : IApp, new()
{
    public delegate void InitializeServicesDelegate(IIoCContainerBuilder builder);

    public static void Run(object args = null, InitializeServicesDelegate initializeServicesDelegate = null)
    {
        RunInternal(args, initializeServicesDelegate);
    }

    private static unsafe void RunInternal(object args, InitializeServicesDelegate initializeServicesDelegate)
    {
        Initialize();
        SDL_Init(SDL_INIT_VIDEO | SDL_INIT_GAMEPAD);
        
        var builder = IoC.IoC.NewBuilder();
        var keyboard = new SdlKeyboard();
        var pointingDevices = new SdlPointingDevices();
        var gameControllers = new SdlGameControllers();
        var eventSource = new EventSource();

        builder
            .WithInstance<IEventSource>(eventSource)
            .WithInstance<IKeyboard>(keyboard)
            .WithInstance<IPointingDevices>(pointingDevices)
            .WithInstance<IGameControllers>(gameControllers)
            .WithPixelCore();
        
        initializeServicesDelegate?.Invoke(builder);

        using var app = new TApp();
        
        var window = SDL_CreateWindow(CString.FromIntPtr(0), 800, 600, 0);
        var appWindowManager = new AppWindowManager(window);
        
        var renderer = SDL_CreateRenderer(window, null);
        var sdlRenderer = new SdlRenderer(renderer);

        builder
            .WithInstance<IRenderer2>(sdlRenderer)
            .WithInstance<IAppWindowManager>(appWindowManager)
            .WithInstance(new SdlHandles(window, renderer));
        
        app.InitializeServices(builder);
        
        using var services = builder.Build();
        app.Initialize(services);
        app.Start(args);

        app.SetActive(true);
        
        var lastTicks = SDL_GetTicksNS();
        
        while (appWindowManager.ShouldContinue)
        {
            SDL_Event @event;
            while(SDL_PollEvent(&@event))
            {
                switch ((SDL_EventType)@event.type)
                {
                    case SDL_EventType.SDL_EVENT_QUIT:
                    {
                        var exitArgs = new WindowClosingEventArgs();
                        appWindowManager.RaiseAppExiting(exitArgs);

                        if (!exitArgs.Cancel)
                        {
                            appWindowManager.Close();
                        }
                    }
                    break;

                    case SDL_EventType.SDL_EVENT_WINDOW_RESIZED:
                    case SDL_EventType.SDL_EVENT_WINDOW_RESTORED:
                    case SDL_EventType.SDL_EVENT_WINDOW_ENTER_FULLSCREEN:
                    case SDL_EventType.SDL_EVENT_WINDOW_LEAVE_FULLSCREEN:
                    {
                        int w, h;
                        SDL_GetRenderOutputSize(renderer, &w, &h);
                        app.Resize(new Size(w, h));
                        break;
                    }
                }
            }
            
            var ticks = SDL_GetTicksNS();
            var dt = (ticks - lastTicks) / 1000000000.0;
            lastTicks = ticks;

            keyboard.Update();
            
            eventSource.OnUpdate((float)dt);
            app.Update((float)dt);
            eventSource.OnUpdated();
            app.Draw(sdlRenderer);
            SDL_RenderPresent(renderer);
            
            eventSource.OnRenderFinished();
        }
        
        SDL_DestroyRenderer(renderer);
        SDL_DestroyWindow(window);
        
        SDL_Quit();
    }
}