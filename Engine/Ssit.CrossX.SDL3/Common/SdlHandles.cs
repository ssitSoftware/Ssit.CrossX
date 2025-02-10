using static bottlenoselabs.Interop.SDL;

namespace Ssit.CrossX.SDL3.Common;

public unsafe class SdlHandles(SDL_Window* window, SDL_Renderer* renderer)
{
    public readonly SDL_Window* Window = window;
    public readonly SDL_Renderer* Renderer = renderer;
}