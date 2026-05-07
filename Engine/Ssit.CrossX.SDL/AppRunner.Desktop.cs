#if !ANDROID && !IOS

using Ssit.CrossX.Core;

namespace Ssit.CrossX.SDL;

public static class AppRunner
{
    public static void Run<TApp>() where TApp : class, IApp, new()
    {
        AppRunnerInternal<TApp>.Run();
    }
}

#endif