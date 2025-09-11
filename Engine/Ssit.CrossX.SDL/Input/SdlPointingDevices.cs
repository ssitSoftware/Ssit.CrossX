using System.Numerics;
using SDL;
using Ssit.CrossX.Input;
using Ssit.CrossX.Input.Internal;
using Ssit.CrossX.SDL.Common;
using static SDL.SDL3;

namespace Ssit.CrossX.SDL.Input;

public unsafe class SdlPointingDevices(SdlHandle<SDL_Window> windowHandle): PointingDevicesBase
{
    private class TouchEntity : ITouchEntity
    {
        public int Id { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Position { get; set; }
        public void Capture(object context)
        {
        }

        public object CapturedBy { get; set; }
        public double InitialTime { get; set; }
        public double Time { get; set; }
    }
    
    private SDL_MouseButtonFlags _previousFlags = 0;
    private Vector2 _previousMousePosition;
    private bool _mouseInWindowLocked = false;

    private readonly TouchEntity[] _mousePointers =
    [
        new TouchEntity {Id = MouseButtons.Left},
        new TouchEntity {Id = MouseButtons.Right},
        new TouchEntity {Id = MouseButtons.Middle}
    ];
    
    public void Update()
    {
        OnPreUpdate();

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
        
        var moved = Math.Abs(_previousMousePosition.X - x) > float.Epsilon || Math.Abs(_previousMousePosition.Y - y) > float.Epsilon;

        ProcessButton(leftButtonState, _mousePointers[0], moved, position);
        ProcessButton(rightButtonState, _mousePointers[1], moved, position);
        ProcessButton(middleButtonState, _mousePointers[2], moved, position);
        
        _previousFlags = state;
        _previousMousePosition = position;
        
        UpdateHoverPosition(focus is null ? null : _previousMousePosition);

        if (_mouseInWindowLocked != LockMouseInWindow)
        {
            SDL_SetWindowMouseGrab(windowHandle.Pointer, LockMouseInWindow);
            _mouseInWindowLocked = LockMouseInWindow;
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

    private void ProcessButton(ButtonState state, TouchEntity entity, bool moved, Vector2 position)
    {
        if (!state.IsChanged && !moved)
            return;
        
        entity.Position = position;
        
        if (!state.IsChanged)
        {
            if (moved && state.IsDown)
            {
                OnMove(entity);
            }
        }
        else
        {
            if (state.IsDown)
            {
                entity.Origin = position;
                OnDown(entity);
            }
            else
            {
                OnUp(entity);
            }
        }
    }
}