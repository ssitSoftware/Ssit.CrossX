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
    
    public static void Run<TApp>(object args = null, Action<IIoCContainerBuilder> registerServices = null) where TApp : class, IApp, new()
    {
        using var app = new TApp();
        AppRunnerInternal.Run(app, args, b =>
        {
            registerServices?.Invoke(b);
            InitializeServices(b);
        });
    }
    
    public static void Run(IApp app, object args = null,  Action<IIoCContainerBuilder> registerServices = null)
    {
        try
        {
            AppRunnerInternal.Run(app, args, b =>
            {
                registerServices?.Invoke(b);
                InitializeServices(b);
            });
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