#if !ANDROID && !IOS

using Ssit.CrossX.Core;
using Ssit.CrossX.Input;
using Ssit.CrossX.SDL.Input;
using Ssit.IoC;

namespace Ssit.CrossX.SDL;

public static class AppRunner
{
    private static void InitializeServices(IIoCContainerBuilder builder)
    {
        builder.WithSingleton<INativeTextInputService, SdlNativeTextInputService>().As<IInternalTextInputService>();
    }
    
    public static void Run<TApp>(object args = null) where TApp : class, IApp, new()
    {
        using var app = new TApp();
        AppRunnerInternal.Run(app, args, InitializeServices);
    }
    
    public static void Run(IApp app, object args = null)
    {
        try
        {
            AppRunnerInternal.Run(app, args, InitializeServices);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            app.Dispose();
        }
    }
}

#endif