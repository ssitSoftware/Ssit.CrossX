using SDL;
using Ssit.CrossX.Input;

namespace Ssit.CrossX.SDL.Input;

internal interface IInternalTextInputService
{
    bool ProcessEvent(SDL_Event @event);
}