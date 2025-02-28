using static bottlenoselabs.Interop.SDL;

namespace Ssit.CrossX.SDL.Common;

public unsafe class SdlHandles(SDL_Window* window, SDL_Renderer* renderer)
{
    // ReSharper disable once UnusedMember.Global
    public readonly SDL_Window* Window = window;
    public readonly SDL_Renderer* Renderer = renderer;
}