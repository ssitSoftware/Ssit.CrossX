#if ANDROID

using Android.App;
using Android.Views;
using Org.Libsdl.App;
using Ssit.CrossX.Core;
using Ssit.CrossX.Input;
using Ssit.CrossX.SDL.Droid.Input;
using Ssit.CrossX.SDL.Services;

namespace Ssit.CrossX.SDL.Droid;

public class CrossXSdlActivity<TApp> : SDLActivity where TApp : class, IApp, new()
{
    private EventSource _eventSource;
    private NativeTextInputServiceDroid _textInputService;

    protected override string[] GetLibraries() => ["SDL3", "SDL3_image", "SDL3_mixer"];

    protected override void Main()
    {
        using var app = new TApp();
        AppRunnerInternal.Run(app, initializeAppDelegate: container =>
        {
            _eventSource = (EventSource)container.Get<IEventSource>();
            _textInputService = container.Get<INativeTextInputService>() as NativeTextInputServiceDroid;
        }, initializeServicesDelegate: builder =>
        {
            builder.WithInstance<Activity>(this);
            builder.WithSingleton<INativeTextInputService, NativeTextInputServiceDroid>();
        });
    }

    public override bool DispatchKeyEvent(KeyEvent e)
    {
        if (e != null && _textInputService?.HandleKeyEvent(e) == true)
            return true;
        return base.DispatchKeyEvent(e);
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
}

#endif
