using Avalonia;
using Ssit.CrossX.XxFormats.Template;

namespace Ssit.CrossX.Editor;

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
            .UsePlatformDetect();
    }
}