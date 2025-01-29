using Avalonia;
using Avalonia.ReactiveUI;
using Ssit.CrossX.Games;

namespace Ssit.CrtossX.Editor;

public static class EditorRunner
{
    public static void Run(string[] args, IGameTemplate gameTemplate)
    {
        var appBuilder = BuildAvaloniaApp(gameTemplate);
        appBuilder.StartWithClassicDesktopLifetime(args);
    }
    
    public static AppBuilder BuildAvaloniaApp(IGameTemplate gameTemplate)
    {
        return AppBuilder.Configure(() => new App(gameTemplate))
            .UseSkia()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI()
            .UsePlatformDetect();
    }
}