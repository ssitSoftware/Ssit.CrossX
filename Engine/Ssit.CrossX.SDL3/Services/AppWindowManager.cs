using Interop.Runtime;
using Ssit.CrossX.Core;
using static bottlenoselabs.Interop.SDL;

namespace Ssit.CrossX.SDL3.Services;

internal unsafe class AppWindowManager(SDL_Window* window): IAppWindowManager
{
    public bool ShouldContinue { get; private set; } = true;
    
    public event Action<WindowClosingEventArgs> Closing;
    
    public void Close()
    {
        ShouldContinue = false;
    }

    public void SetFullscreen()
    {
        SDL_SetWindowFullscreen(window, CBool.FromBoolean(true));
    }

    public void SetWindowed(Size size)
    {
        SDL_SetWindowFullscreen(window, CBool.FromBoolean(false));
        SDL_SetWindowSize(window, size.Width, size.Height);
    }

    public void  SetTitle(string title)
    {
        using var allocator = new ArenaNativeAllocator(2048);
        SDL_SetWindowTitle(window, allocator.AllocateCString(title));
    }

    public void RaiseAppExiting(WindowClosingEventArgs args)
    {
        Closing?.Invoke(args);
    }
}