using System.Numerics;
using SDL;
using Ssit.CrossX.Input;
using Ssit.CrossX.Input.Internal;
using Ssit.CrossX.SDL.Common;
using static SDL.SDL3;

namespace Ssit.CrossX.SDL.Input;

public unsafe class SdlPointingDevices : PointingDevicesBase
{
    public override PointingDevicesMode Mode
    {
        get => base.Mode;
        set
        {
            var val = value;
            if (_touchDevices.Count == 0)
            {
                val &= ~PointingDevicesMode.Touch;
            }
            base.Mode = val;
        }
    }

    private SDL_MouseButtonFlags _previousFlags = 0;
    private Vector2 _previousMousePosition;
    private bool _mouseInWindowLocked = false;

    private int _nextId = MouseButtons.Middle + 1;
    private readonly Dictionary<SDL_FingerID, int> _touchIds = new();
    private readonly List<SDL_FingerID> _fingers = [];
    
    private readonly SdlHandle<SDL_Window> _windowHandle;
    private readonly List<SDL_TouchID> _touchDevices = [];

    public SdlPointingDevices(SdlHandle<SDL_Window> windowHandle)
    {
        _windowHandle = windowHandle;

        var platform = SDL_GetPlatform()?.ToLowerInvariant();
        if (platform is "android" or "ios")
        {
            using var array = SDL_GetTouchDevices();
            if (array is not null)
            {
                for (var idx = 0; idx < array.Count; idx++)
                {
                    var type = SDL_GetTouchDeviceType(array[idx]);
                    var name = SDL_GetTouchDeviceName(array[idx]);
                    
                    if (type == SDL_TouchDeviceType.SDL_TOUCH_DEVICE_DIRECT)
                    {
                        _touchDevices.Add(array[idx]);
                    }
                }
            }
        }
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
            //ProcessTouch();
        }
    }

    private void ProcessTouch()
    {
        if (_touchDevices.Count == 0)
            return;
        
        for(var idx =0; idx < _fingers.Count;)
        {
            var pointerId = GetTouchId((ulong)_fingers[idx]);
            var pointer = GetPointer(pointerId);
            
            if (pointer != null)
            {
                if (pointer.State.IsDown)
                { 
                    SetPointer(pointerId, ButtonState.JustReleased, null);
                }
                ++idx;
            }
            else
            {
                _fingers.RemoveAt(idx);
            }
        }

        foreach (var touchDeviceId in _touchDevices)
        {
            using var array = SDL_GetTouchFingers(touchDeviceId);
            if (array != null)
            {
                for (var idx = 0; idx < array.Count; idx++)
                {
                    var finger = array[idx];
                    var id = GetTouchId((ulong)finger.id);

                    var pointer = GetPointer(id);
                    if (pointer == null)
                    {
                        SetPointer(id, ButtonState.JustPressed, new Vector2(finger.x, finger.y));
                        _fingers.Add(finger.id);
                    }
                    else
                    {
                        SetPointer(id, ButtonState.Down, new Vector2(finger.x, finger.y));
                    }
                }
            }
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