#if ANDROID

using Org.Libsdl.App;
using Ssit.CrossX.Core;
using Ssit.CrossX.SDL.Services;

namespace Ssit.CrossX.SDL.Droid;

public class CrossXSdlActivity<TApp> : SDLActivity where TApp : class, IApp, new()
{
    private EventSource _eventSource;

    protected override string[] GetLibraries() => ["SDL3", "SDL3_image", "SDL3_mixer"];

    protected override void Main()
    {
        using var app = new TApp();
        AppRunnerInternal.Run(app, initializeAppDelegate: container =>
        {
            _eventSource = (EventSource)container.Get<IEventSource>();
        });
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