using System.Numerics;
using SDL;
using Ssit.CrossX.Input;
using Ssit.CrossX.Input.Internal;
using Ssit.CrossX.SDL.Common;
using static SDL.SDL3;

namespace Ssit.CrossX.SDL.Input;

public unsafe partial class SdlPointingDevices : PointingDevicesBase
{
    public override PointingDevicesMode Mode
    {
        get => base.Mode;
        set
        {
            var val = value;
            
            var platform = SDL_GetPlatform()?.ToLowerInvariant();
            if (platform is not ("android" or "ios"))
            {
                val &= ~PointingDevicesMode.Touch;
            }
            
            base.Mode = val;
        }
    }

    private SDL_MouseButtonFlags _previousFlags = 0;
    private Vector2 _previousMousePosition;
    private bool _mouseInWindowLocked;

    private readonly List<SDL_FingerID> _fingers = [];
    private readonly SdlHandle<SDL_Window> _windowHandle;

    public SdlPointingDevices(SdlHandle<SDL_Window> windowHandle): this()
    {
        _windowHandle = windowHandle;
    }

    public void Update()
    {
        OnPreUpdate();

        if ((Mode & PointingDevicesMode.Mouse) != 0)
        {
            float x, y;
            var state = SDL_GetMouseState(&x, &y);
            var focus = SDL_GetMouseFocus();
            var position = new Vector2(x, y);

            var leftButtonState = ButtonState.FromStates(state.HasFlag(SDL_MouseButtonFlags.SDL_BUTTON_LMASK),
                _previousFlags.HasFlag(SDL_MouseButtonFlags.SDL_BUTTON_LMASK));

            var rightButtonState = ButtonState.FromStates(state.HasFlag(SDL_MouseButtonFlags.SDL_BUTTON_RMASK),
                _previousFlags.HasFlag(SDL_MouseButtonFlags.SDL_BUTTON_RMASK));

            var middleButtonState = ButtonState.FromStates(state.HasFlag(SDL_MouseButtonFlags.SDL_BUTTON_MMASK),
                _previousFlags.HasFlag(SDL_MouseButtonFlags.SDL_BUTTON_MMASK));

            var moved = Math.Abs(_previousMousePosition.X - x) > float.Epsilon ||
                        Math.Abs(_previousMousePosition.Y - y) > float.Epsilon;

            ProcessButton(leftButtonState, MouseButtons.Left, moved, position);
            ProcessButton(rightButtonState, MouseButtons.Right, moved, position);
            ProcessButton(middleButtonState, MouseButtons.Middle, moved, position);
            
            _previousFlags = state;
            _previousMousePosition = position;
        
            UpdateHoverPosition(focus is null ? null : _previousMousePosition);
            
            if (_mouseInWindowLocked != LockMouseInWindow)
            {
                SDL_SetWindowMouseGrab(_windowHandle.Pointer, LockMouseInWindow);
                _mouseInWindowLocked = LockMouseInWindow;
            }
        }

        if ((Mode & PointingDevicesMode.Touch) != 0)
        {
            ProcessTouch();
        }
    }

    protected override void ShowPointer(bool show)
    {
        if (show)
        {
            SDL_ShowCursor();
        }
        else
        {
            SDL_HideCursor();
        }
    }

    private void ProcessButton(ButtonState state, int id, bool moved, Vector2 position)
    {
        if (!state.IsChanged && !moved)
            return;
        
        if (!state.IsChanged)
        {
            if (moved && state.IsDown)
            {
                SetPointer(id, ButtonState.Down, position);
            }
        }
        else
        {
            if (state.IsDown)
            {
                SetPointer(id, ButtonState.JustPressed, position);
            }
            else
            {
                SetPointer(id, ButtonState.JustReleased, position);
            }
        }
    }
}