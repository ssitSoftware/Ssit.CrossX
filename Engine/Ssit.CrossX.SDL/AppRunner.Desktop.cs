#if !ANDROID && !IOS

using Ssit.CrossX.Core;

namespace Ssit.CrossX.SDL;

public static class AppRunner
{
    public static void Run<TApp>(object args = null) where TApp : class, IApp, new()
    {
        using var app = new TApp();
        AppRunnerInternal.Run(app, args);
    }
    
    public static void Run(IApp app, object args = null)
    {
        try
        {
            AppRunnerInternal.Run(app, args);
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