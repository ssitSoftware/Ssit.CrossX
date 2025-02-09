using System.Diagnostics;
using Ssit.CrossX.Core;
using Ssit.CrossX.IoC;
using Ssit.CrossX.SDL3.Services;

namespace Ssit.CrossX.SDL3;

public static class AppRunner<TApp> where TApp : IApp, new()
{
    public delegate void InitializeServicesDelegate(IIoCContainerBuilder builder);

    public static Task Run(object args, InitializeServicesDelegate initializeServicesDelegate = null)
    {
        return Task.Run( () => RunInternal(args, initializeServicesDelegate) );
    }

    private static void RunInternal(object args, InitializeServicesDelegate initializeServicesDelegate)
    {
        var stopWatch = new Stopwatch();
        
        var lifetimeManager = new LifeTimeManager();
        
        var builder = IoC.IoC.NewBuilder();
        builder.WithInstance<IAppLifetimeManager>(lifetimeManager);
        
        initializeServicesDelegate?.Invoke(builder);

        using var app = new TApp();
        app.InitializeServices(builder);
        
        using var services = builder.Build();
        app.Initialize(services);
        app.Start(args);

        app.SetActive(true);

        stopWatch.Restart();
        var lastTime = stopWatch.Elapsed;
        
        while (lifetimeManager.ShouldContinue)
        {
            var time = stopWatch.Elapsed;
            var dt = time - lastTime; 
            lastTime = time;
            
            app.Update((float)dt.TotalSeconds);
            app.Draw();
        }

        lifetimeManager.RaiseAppExiting();
    }
}