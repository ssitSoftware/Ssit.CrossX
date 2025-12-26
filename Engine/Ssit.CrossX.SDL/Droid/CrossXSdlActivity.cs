#if ANDROID

using System.Numerics;
using Android.Views;
using Org.Libsdl.App;
using Ssit.CrossX.Core;
using Ssit.CrossX.Input;
using Ssit.CrossX.Input.Internal;
using Ssit.CrossX.SDL.Services;

namespace Ssit.CrossX.SDL.Droid;

public class CrossXSdlActivity<TApp>: SDLActivity where TApp: class, IApp, new()
{
    private IInputHandler _inputHandler;
    private EventSource _eventSource;
    
    private readonly int[] _viewLocation = new int[2];
    
    protected override string[] GetLibraries() => ["SDL3", "SDL3_image", "SDL3_mixer"];
    protected override void Main() => AppRunner<TApp>.Run( initializeAppDelegate: container =>
    {
        _inputHandler = container.Get<IInputHandler>();
        _eventSource = (EventSource)container.Get<IEventSource>();
    });
    
    private Vector2 GetPointerPosition(int index, MotionEvent evnt, out int id)
    {
        float viewX = evnt.GetX(index);
        float viewY = evnt.GetY(index);

        var position = new Vector2(viewX, viewY);

        id = evnt.GetPointerId(index) + 1;
        return position;
    }

    protected override void OnPause()
    {
        base.OnPause();
        _eventSource?.OnPause();
    }

    protected override void OnResume()
    {
        base.OnResume();
        _eventSource?.OnResume();
    }

    public override bool DispatchTouchEvent(MotionEvent @event)
    {
        Window!.DecorView!.RootView!.GetLocationOnScreen(_viewLocation);
        
        switch (@event!.ActionMasked)
        {
            case MotionEventActions.Down:
            case MotionEventActions.PointerDown:
            {
                var position = GetPointerPosition(@event.ActionIndex, @event, out var id);
                _inputHandler.OnTouch((ulong)id, ButtonState.JustPressed, position);
            }
                break;

            case MotionEventActions.Move:
                for (int index = 0; index < @event.PointerCount; index++)
                {
                    var position = GetPointerPosition(index, @event, out var id);
                    _inputHandler.OnTouch((ulong)id, ButtonState.Down, position);
                }

                break;

            case MotionEventActions.Up:
            case MotionEventActions.PointerUp:
            {
                var position = GetPointerPosition(@event.ActionIndex, @event, out var id);
                _inputHandler.OnTouch((ulong)id, ButtonState.JustReleased, position);
            }
                break;

            case MotionEventActions.Cancel:
            case MotionEventActions.Outside:
            {
                for (int index = 0; index < @event.PointerCount; index++)
                {
                    GetPointerPosition(index, @event, out var id);
                    _inputHandler.OnTouch((ulong)id, ButtonState.JustReleased, null);
                }
            }
                break;
        }

        return true;
    }
}

#endif