using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Ssit.Pixel.Framework.Core;
using Ssit.Pixel.Framework.Core.Internal;
using Ssit.Pixel.Framework.Input;
using Ssit.Pixel.Framework.IoC;
using Ssit.Pixel.NET.Input;
using static SDL2.Bindings.SDL;

namespace Ssit.Pixel.NET.Internal;

public abstract class PixelAppDesktopRunner<TApp>: PixelAppRunnerBase<TApp> where TApp: PixelApp
{
    private WindowParameters _windowParameters;
    protected IntPtr SdlRendererHandle;
    private IntPtr _windowHandle;
    
    private KeyboardImpl _keyboard;
    private GameControllersImpl _gameControllers;
    
    protected virtual SDL_WindowFlags AdditionalWindowFlags => 0;
    
    protected override void Loop(TApp app)
    {
        bool running = true;
        float dt;

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var lastStopwatchRead = stopwatch.Elapsed;

        bool isActive = false;

        TimeSpan cursorShowTime = stopwatch.Elapsed;
        
        while (running)
        {
            while (SDL_PollEvent(out SDL_Event e) == 1)
            {
                switch (e.type)
                {
                    case SDL_EventType.SDL_QUIT:
                        running = false;
                        break;
                    
                    case SDL_EventType.SDL_MOUSEMOTION:
                        SDL_ShowCursor(1);
                        cursorShowTime = stopwatch.Elapsed;
                        break;
                    
                    case SDL_EventType.SDL_KEYDOWN:
                        if (e.key.keysym.scancode == SDL_Scancode.SDL_SCANCODE_F4)
                        {
                            _windowParameters.FullScreen = !_windowParameters.FullScreen;
                            _windowParameters.Apply();
                        }
                        break;
                    
                    case SDL_EventType.SDL_WINDOWEVENT:
                        switch (e.window.windowEvent)
                        {
                            case SDL_WindowEventID.SDL_WINDOWEVENT_RESIZED:
                            case SDL_WindowEventID.SDL_WINDOWEVENT_MAXIMIZED:
                            case SDL_WindowEventID.SDL_WINDOWEVENT_RESTORED:
                                _windowParameters.Width = 0;
                                _windowParameters.Height = 0;
                                break;
                            
                            case SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_LOST:
                                isActive = false;
                                ActivateApp(app, false);
                                break;
                            
                            case SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_GAINED:
                                isActive = true;
                                ActivateApp(app, true);
                                break;
                        }
                        break;
                }
                _gameControllers.ProcessEvent(e);
            }

            UpdateParameters();
            
            var stopwatchRead = stopwatch.Elapsed;
            dt = (float)(stopwatchRead - lastStopwatchRead).TotalSeconds;
            lastStopwatchRead = stopwatchRead;
            
            if ((stopwatchRead - cursorShowTime).Milliseconds > 500 && isActive)
            {
                SDL_ShowCursor(0);
            }
            
            _keyboard.PreUpdate();

            UpdateApp(app, dt);
            
            SDL_SetRenderDrawColor(SdlRendererHandle, 0, 0, 0, 255);
            SDL_RenderClear(SdlRendererHandle);

            DrawApp(app);
            
            SDL_RenderPresent(SdlRendererHandle);
            
            _gameControllers.PostUpdate();

            if (!isActive)
            {
                Thread.Sleep(30);
            }
        }

        stopwatch.Stop();
    }

    protected override void InitializePlatform(IIoCContainerBuilder builder, WindowParameters parameters)
    {
        _windowParameters = parameters;
        
        SDL_Init(SDL_INIT_VIDEO | SDL_INIT_EVENTS | SDL_INIT_GAMECONTROLLER | SDL_INIT_AUDIO);
        
        _gameControllers = new GameControllersImpl();
        _keyboard = new KeyboardImpl();

        builder
            .WithInstance<IGameControllers>(_gameControllers)
            .WithInstance<IKeyboard>(_keyboard);

        SDL_WindowFlags flags = SDL_WindowFlags.SDL_WINDOW_SHOWN | AdditionalWindowFlags;
        
        if (parameters.AllowResize)
        {
            flags |= SDL_WindowFlags.SDL_WINDOW_RESIZABLE;
        }
        
        if (parameters.FullScreen)
        {
            flags |= SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP;
        }
        
        _windowHandle = SDL_CreateWindow("Test", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED,
            parameters.Width, parameters.Height, flags);

        InitializeRenderer(_windowHandle, builder);
        parameters.ApplyParameters += WindowOnApplyParameters;
    }

    protected virtual void InitializeRenderer(IntPtr windowHandle, IIoCContainerBuilder builder)
    {
        SdlRendererHandle = SDL_CreateRenderer(windowHandle, 0,
            SDL_RendererFlags.SDL_RENDERER_ACCELERATED |
            SDL_RendererFlags.SDL_RENDERER_TARGETTEXTURE |
            SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);
    }
    
    private void UpdateParameters()
    {
        if (!_windowParameters.HasChanges)
            return;
        
        var flags = (SDL_WindowFlags)SDL_GetWindowFlags(_windowHandle);

        bool isFullscreen = flags.HasFlag(SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP);
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            isFullscreen = flags.HasFlag(SDL_WindowFlags.SDL_WINDOW_MAXIMIZED);
        }
        
        bool allowResize = flags.HasFlag(SDL_WindowFlags.SDL_WINDOW_RESIZABLE);
        
        _windowParameters.FullScreen = isFullscreen;
        _windowParameters.AllowResize = allowResize;
        
        SDL_GetWindowSize(_windowHandle, out var w, out var h);
        
        _windowParameters.Width = w;
        _windowParameters.Height = h;
        
        _windowParameters.Apply(false);
    }
    
    private void WindowOnApplyParameters()
    {
        var flags = (SDL_WindowFlags)SDL_GetWindowFlags(_windowHandle);

        bool isFullscreen = flags.HasFlag(SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP);
        bool allowResize = flags.HasFlag(SDL_WindowFlags.SDL_WINDOW_RESIZABLE);
        
        if (allowResize != _windowParameters.AllowResize)
        {
            SDL_SetWindowResizable(_windowHandle,
                _windowParameters.AllowResize ? SDL_bool.SDL_TRUE : SDL_bool.SDL_FALSE);
        }
        
        SDL_SetWindowSize(_windowHandle, _windowParameters.Width, _windowParameters.Height);
        
        if (isFullscreen != _windowParameters.FullScreen)
        {
            SDL_SetWindowFullscreen(_windowHandle,
                isFullscreen ? 0 : (uint) SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP);
        }
    }
}