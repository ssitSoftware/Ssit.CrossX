using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SDL;
using Ssit.CrossX.Audio;
using Ssit.CrossX.Audio.Internal;
using Ssit.CrossX.Core;
using Ssit.CrossX.Core.Internal;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.Input;
using Ssit.CrossX.Input.Internal;
using Ssit.IoC;
using Ssit.CrossX.SDL.Audio;
using Ssit.CrossX.SDL.Common;
using Ssit.CrossX.SDL.Graphics;
using Ssit.CrossX.SDL.Input;
using Ssit.CrossX.SDL.Services;

using static SDL.SDL3;

namespace Ssit.CrossX.SDL;

internal static class AppEventWatcher
{
    public static event Action AppActivated;
    public static event Action AppDeativated;
    
    public static void CallAppActivated() => AppActivated?.Invoke();
    public static void CallAppDeativated() => AppDeativated?.Invoke();
    
    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe SDLBool AppEventWatch(IntPtr _, SDL_Event* eventPtr)
    {
        if (eventPtr->type == (uint)SDL_EventType.SDL_EVENT_DID_ENTER_BACKGROUND)
        {
            CallAppDeativated();
        }
        
        if (eventPtr->type == (uint)SDL_EventType.SDL_EVENT_DID_ENTER_FOREGROUND)
        {
            CallAppActivated();
        }
        return false;
    }
}

internal static class AppRunnerInternal<TApp> where TApp : class, IApp, new()
{
    public delegate void InitializeServicesDelegate(IIoCContainerBuilder builder);

    public delegate void InitializeAppDelegate(IIoCContainer container);

    public static void Run(object args = null, InitializeServicesDelegate initializeServicesDelegate = null, InitializeAppDelegate initializeAppDelegate = null)
    {
        RunInternal(args, initializeServicesDelegate, initializeAppDelegate);
    }

    private static unsafe void RunInternal(object args, InitializeServicesDelegate initializeServicesDelegate,
        InitializeAppDelegate initializeAppDelegate)
    {
        SDL_Init(SDL_InitFlags.SDL_INIT_VIDEO | SDL_InitFlags.SDL_INIT_GAMEPAD | SDL_InitFlags.SDL_INIT_AUDIO | SDL_InitFlags.SDL_INIT_HAPTIC);

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
            .WithSingleton<ISoundManager, SdlSoundManagerImpl>().As<SdlSoundManagerImpl>()
            .WithSingleton<SdlTrackPool, SdlTrackPool>()
            .WithImplementation<ISoundEffect, SdlSoundEffectImpl>()
            .WithImplementation<ISingleMusicPlayer, SdlSingleMusicPlayer>()
            .WithSingleton<IHapticDevice, SdlHapticDevice>()
            .WithPixelCore();

        initializeServicesDelegate?.Invoke(builder);

        using var app = new TApp();

        SDL_WindowFlags flags = 0;

        var size = new Size(800, 480);
        if (app.IsPortrait)
        {
            SDL_SetHint(SDL_HINT_ORIENTATIONS, "Portrait");
            size = new Size(480, 800);
        }
        
#if IOS
        SDL_SetHint( SDL_HINT_IOS_HIDE_HOME_INDICATOR, "2" );
        flags = SDL_WindowFlags.SDL_WINDOW_BORDERLESS | SDL_WindowFlags.SDL_WINDOW_FULLSCREEN |
                SDL_WindowFlags.SDL_WINDOW_HIGH_PIXEL_DENSITY | SDL_WindowFlags.SDL_WINDOW_METAL;
#elif ANDROID
        flags = SDL_WindowFlags.SDL_WINDOW_BORDERLESS | SDL_WindowFlags.SDL_WINDOW_FULLSCREEN |
                SDL_WindowFlags.SDL_WINDOW_HIGH_PIXEL_DENSITY;
#endif
        
        var window = SDL_CreateWindow("", size.Width, size.Height, flags);
        var renderer = SDL_CreateRenderer(window, (byte*)null);
        SDL_SetRenderVSync(renderer, 1);
        
        var pointingDevices = new SdlPointingDevices(new SdlHandle<SDL_Window>(window));
        
        var appWindowManager = new AppWindowManager(window, renderer);
        var sdlRenderer = new SdlRenderer(renderer);
        
        builder
            .WithInstance<IRenderer2>(sdlRenderer)
            .WithInstance<IAppWindowManager>(appWindowManager)
            .WithInstance<IPointingDevices>(pointingDevices).As<IInputHandler>()
            .WithInstance(new SdlHandles(window, renderer));
        
        app.InitializeServices(builder);

        if (builder.IsRegistered(typeof(IPaletteSource)))
        {
            builder.WithSingleton<ISdlPalette, SdlPalette>();
        }
        
        var services = builder.Build();

        var actionScheduler = services.Get<IActionScheduler>();

        var updatables = services.Fetch<IUpdatable>().ToArray();
        
        appWindowManager.Initialize(actionScheduler);
        
        initializeAppDelegate?.Invoke(services);
        
        app.Initialize(services);
        app.Start(args);

        (actionScheduler as IInternalActionScheduler)?.Process();
        
        app.SetActive(true);
        
        if ((pointingDevices.Mode & PointingDevicesMode.Mouse) == 0)
        {
            SDL_HideCursor();
        }
        
        var lastTicks = SDL_GetTicksNS();
        bool shouldDisplayAndUpdate = true;

        eventSource.Paused += () => shouldDisplayAndUpdate = false;
        eventSource.Resumed += () =>
        {
            shouldDisplayAndUpdate = true;
            lastTicks = SDL_GetTicksNS();
        };
        
        AppEventWatcher.AppActivated += eventSource.OnResume;
        AppEventWatcher.AppDeativated += eventSource.OnPause;
        
        while (appWindowManager.ShouldContinue)
        {
            SDL_Event @event;
            while (SDL_PollEvent(&@event))
            {
                switch ((SDL_EventType)@event.type)
                {
                    case SDL_EventType.SDL_EVENT_QUIT:
                    {
                        var exitArgs = new WindowClosingEventArgs();
                        appWindowManager.RaiseAppExiting(exitArgs);

                        if (!exitArgs.Cancel)
                        {
                            Console.WriteLine("[AppRunner] SDL_EVENT_QUIT received — closing app");
                            appWindowManager.Close();
                            shouldDisplayAndUpdate = false;
                        }
                    }
                    break;

#if !ANDROID && !IOS
                    case SDL_EventType.SDL_EVENT_WINDOW_FOCUS_LOST:
                        eventSource.OnPause();
                        break;
                    
                    case SDL_EventType.SDL_EVENT_WINDOW_FOCUS_GAINED:
                        eventSource.OnResume();
                        app.SetActive(true);
                        break;
#endif
                    
                    case SDL_EventType.SDL_EVENT_DID_ENTER_BACKGROUND:
                        shouldDisplayAndUpdate = false;
                        eventSource.OnPause();
                        app.SetActive(false);
                        break;
                    
                    case SDL_EventType.SDL_EVENT_WILL_ENTER_BACKGROUND:
                        shouldDisplayAndUpdate = false;
                        eventSource.OnPause();
                        app.SetActive(false);
                        break;
                    
                    case SDL_EventType.SDL_EVENT_TERMINATING:
                        Console.WriteLine("[AppRunner] SDL_EVENT_TERMINATING received — closing app");
                        shouldDisplayAndUpdate = false;
                        appWindowManager.Close();
                        break;
                    
                    case SDL_EventType.SDL_EVENT_DID_ENTER_FOREGROUND:
                        shouldDisplayAndUpdate = true;
                        eventSource.OnResume();
                        app.SetActive(true);
                        break;

                    case SDL_EventType.SDL_EVENT_WINDOW_RESIZED:
                    case SDL_EventType.SDL_EVENT_WINDOW_RESTORED:
                    case SDL_EventType.SDL_EVENT_WINDOW_ENTER_FULLSCREEN:
                    case SDL_EventType.SDL_EVENT_WINDOW_LEAVE_FULLSCREEN:
                    {
                        SDL_SetRenderViewport(renderer, null);
                        app.Resize(sdlRenderer.TargetSize);
                        appWindowManager.EnsureWindowSize();

                        SDL_SetWindowMouseGrab(window, false);
                        SDL_SetWindowMouseGrab(window, pointingDevices.LockMouseInWindow);
                        
                        if ((pointingDevices.Mode & PointingDevicesMode.Mouse) == 0)
                        {
                            SDL_HideCursor();
                        }
                        break;
                    }
                    
                    case SDL_EventType.SDL_EVENT_WINDOW_MOUSE_LEAVE:
                        SDL_ShowCursor();
                        break;
                    
                    case SDL_EventType.SDL_EVENT_WINDOW_MOUSE_ENTER:
                        if ((pointingDevices.Mode & PointingDevicesMode.Mouse) == 0)
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
            
            if (!shouldDisplayAndUpdate) continue;
            
            var ticks = SDL_GetTicksNS();
            var dt = (ticks - lastTicks) / 1000000000.0;
            lastTicks = ticks;
            
            pointingDevices.Update();
            keyboard.Update();
            
            eventSource.OnUpdate((float)dt);
            
            foreach(var updatable in updatables)
            {
                updatable.Update((float)dt);
            }
            
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