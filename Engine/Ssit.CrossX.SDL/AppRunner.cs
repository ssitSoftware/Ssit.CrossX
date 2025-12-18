using SDL;
using Ssit.CrossX.Audio;
using Ssit.CrossX.Core;
using Ssit.CrossX.Core.Internal;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.Input;
using Ssit.IoC;
using Ssit.CrossX.SDL.Audio;
using Ssit.CrossX.SDL.Common;
using Ssit.CrossX.SDL.Graphics;
using Ssit.CrossX.SDL.Input;
using Ssit.CrossX.SDL.Services;

using static SDL.SDL3;

namespace Ssit.CrossX.SDL;

public static class AppRunner<TApp> where TApp : class, IApp, new()
{
    public delegate void InitializeServicesDelegate(IIoCContainerBuilder builder);

    public static void Run(object args = null, InitializeServicesDelegate initializeServicesDelegate = null)
    {
        RunInternal(args, initializeServicesDelegate);
    }

    private static unsafe void RunInternal(object args, InitializeServicesDelegate initializeServicesDelegate)
    {
        SDL_Init(SDL_InitFlags.SDL_INIT_VIDEO | SDL_InitFlags.SDL_INIT_GAMEPAD |  SDL_InitFlags.SDL_INIT_AUDIO);
        
        var builder = IoC.IoC.NewBuilder();
        var keyboard = new SdlKeyboard();
        var gameControllers = new SdlGameControllers();
        var eventSource = new EventSource();

        builder
            .WithInstance<IEventSource>(eventSource)
            .WithInstance<IKeyboard>(keyboard)
            .WithInstance<IGameControllers>(gameControllers)
            .WithImplementation<ITexture, SdlTexture>()
            .WithImplementation<IRenderTarget, SdlRenderTarget>()
            .WithImplementation<IVertexBuffer, SdlVertexBuffer>()
            .WithSingleton<ISoundManager, SdlSoundManagerImpl>()
            .WithImplementation<ISoundEffect, SdlSoundEffectImpl>()
            .WithSingleton<IMusicPlayer, SdlMusicPlayer>()
            .WithPixelCore();
        
        initializeServicesDelegate?.Invoke(builder);

        using var app = new TApp();
        
        var window = SDL_CreateWindow("", 800, 600, 0);
        var renderer = SDL_CreateRenderer(window, (byte*)null);
        SDL_SetRenderVSync(renderer, 1);
        
        var pointingDevices = new SdlPointingDevices(new SdlHandle<SDL_Window>(window));
        
        var appWindowManager = new AppWindowManager(window, renderer);
        var sdlRenderer = new SdlRenderer(renderer);

        builder
            .WithInstance<IRenderer2>(sdlRenderer)
            .WithInstance<IAppWindowManager>(appWindowManager)
            .WithInstance<IPointingDevices>(pointingDevices)
            .WithInstance(new SdlHandles(window, renderer));
        
        app.InitializeServices(builder);

        if (builder.IsRegistered(typeof(IPaletteSource)))
        {
            builder.WithSingleton<ISdlPalette, SdlPalette>();
        }
        
        var services = builder.Build();
        
        var actionScheduler = services.Get<IActionScheduler>();
        appWindowManager.Initialize(actionScheduler);
        
        app.Initialize(services);
        app.Start(args);

        (actionScheduler as IInternalActionScheduler)?.Process();
        
        app.SetActive(true);
        
        if (!pointingDevices.Enable)
        {
            SDL_HideCursor();
        }
        
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
                        SDL_Rect vp = new() { x = 0, y = 0, w = w, h = h };
                        SDL_SetRenderViewport(renderer, &vp);
                        app.Resize(new Size(w, h));
                        appWindowManager.EnsureWindowSize();

                        SDL_SetWindowMouseGrab(window, false);
                        SDL_SetWindowMouseGrab(window, pointingDevices.LockMouseInWindow);
                        
                        if (!pointingDevices.Enable)
                        {
                            SDL_HideCursor();
                        }
                        break;
                    }
                    
                    case SDL_EventType.SDL_EVENT_WINDOW_MOUSE_LEAVE:
                        SDL_ShowCursor();
                        break;
                    
                    case SDL_EventType.SDL_EVENT_WINDOW_MOUSE_ENTER:
                        if (!pointingDevices.Enable)
                        {
                            SDL_HideCursor();
                        }
                        break;
                    
                    case SDL_EventType.SDL_EVENT_KEY_DOWN:
                        (app as IKeyboardEventHandler)?.OnKeyDown((Key)@event.key.scancode);
                        break;
                    
                    case SDL_EventType.SDL_EVENT_KEY_UP:
                        (app as IKeyboardEventHandler)?.OnKeyUp((Key)@event.key.scancode);
                        break;
                }
                
                gameControllers.ProcessEvent(@event);
            }
            
            var ticks = SDL_GetTicksNS();
            var dt = (ticks - lastTicks) / 1000000000.0;
            lastTicks = ticks;

            pointingDevices.Update();
            keyboard.Update();
            
            eventSource.OnUpdate((float)dt);
            app.Update((float)dt);
            eventSource.OnUpdated();
            gameControllers.PostUpdate();
            
            sdlRenderer.ResetStats();
            
            app.Draw(sdlRenderer);
            SDL_RenderPresent(renderer);
            eventSource.OnRenderFinished();
        }
        
        services.Dispose();
        
        SDL_DestroyRenderer(renderer);
        SDL_DestroyWindow(window);
        
        SDL_Quit();
    }
}