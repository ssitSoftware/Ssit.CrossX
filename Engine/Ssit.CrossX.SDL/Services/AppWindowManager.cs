using SDL;
using Ssit.CrossX.Core;
using static SDL.SDL3;

namespace Ssit.CrossX.SDL.Services;

internal unsafe class AppWindowManager(SDL_Window* window, SDL_Renderer* renderer): IAppWindowManager
{
    private IActionScheduler _actionScheduler;
    public bool ShouldContinue { get; private set; } = true;

    private Size _windowSize = new Size(800, 600);
    
    public event Action<WindowClosingEventArgs> Closing;

    public void Initialize(IActionScheduler actionScheduler)
    {
        _actionScheduler = actionScheduler;
    }
    
    public void Close()
    {
        ShouldContinue = false;
    }

    public void SetFullscreen()
    {
        _actionScheduler.Schedule(() =>
            {
                var flags = SDL_GetWindowFlags(window);
                if ((flags & SDL_WindowFlags.SDL_WINDOW_FULLSCREEN) == 0)
                {
                    SDL_SetWindowFullscreen(window, true);
                }
            }
        );
    }

    public void SetWindowed(Size size)
    {
        _actionScheduler.Schedule(() =>
        {
            var flags = SDL_GetWindowFlags(window);
            if ((flags & SDL_WindowFlags.SDL_WINDOW_FULLSCREEN) != 0)
            {
                SDL_SetWindowFullscreen(window, false);
            }

            _windowSize = size;
            SDL_SetWindowSize(window, size.Width, size.Height);
            SDL_SetWindowPosition(window, (int)SDL_WINDOWPOS_CENTERED, (int)SDL_WINDOWPOS_CENTERED);
        });
    }

    public void EnsureWindowSize()
    {
        var flags = SDL_GetWindowFlags(window);
        if ((flags & SDL_WindowFlags.SDL_WINDOW_FULLSCREEN) != 0)
        {
            return;
        }
        
        int w, h;
        SDL_GetRenderOutputSize(renderer, &w, &h);

        if (w != _windowSize.Width || h != _windowSize.Height)
        {
            SDL_SetWindowSize(window, _windowSize.Width, _windowSize.Height);
            SDL_SetWindowPosition(window, (int)SDL_WINDOWPOS_CENTERED, (int)SDL_WINDOWPOS_CENTERED);
        }
    }

    public void SetTitle(string title) => SDL_SetWindowTitle(window, title);

    public void RaiseAppExiting(WindowClosingEventArgs args)
    {
        Closing?.Invoke(args);
    }
}