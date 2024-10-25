using System;
using Ssit.Utils.IoC;

namespace Ssit.Pixel.Framework.Core.Internal;

public abstract class PixelAppRunnerBase<TApp> where TApp: PixelApp
{
    ~PixelAppRunnerBase()
    {
        OnDispose(false);
    }

    protected void RunInternal(WindowParameters parameters, IIoCContainerBuilder builder)
    {
        parameters ??= new WindowParameters();
        
        builder ??= IoC.NewBuilder();
        
        InitializePlatform(builder, parameters);
        
        var appData = InitializeApp(parameters, builder, PostInitialize);

        appData.app.Start(null);
        Loop(appData.app);
        
        appData.app.Dispose();
        
        OnDispose(true);
        
        appData.c1.Dispose();
        appData.c0.Dispose();
    }

    protected virtual void PostInitialize(IIoCContainerBuilder builder, TApp app)
    {
    }

    protected abstract void Loop(TApp app);

    protected void UpdateApp(TApp app, float dt) => app.Update(dt);
    protected void DrawApp(TApp app) => app.Draw();

    protected void ActivateApp(TApp app, bool active) => app.SetActive(active);

    protected abstract void InitializePlatform(IIoCContainerBuilder builder, WindowParameters parameters);

    private (IIoCContainer c1, IIoCContainer c0, TApp app) InitializeApp(
        WindowParameters parameters,
        IIoCContainerBuilder builder, 
        Action<IIoCContainerBuilder, TApp> postConfigure)
    {
        var container = builder.WithPixelCore().Build();
        var app = container.IoCConstruct<TApp>(parameters);
        
        var newBuilder = IoC.NewBuilder()
            .WithParent(container);

        var newContainer = app.InitializeServices(newBuilder, b => postConfigure(b, app));
        
        return (newContainer, container, app);
    }
    
    protected virtual void OnDispose(bool disposing)
    {
    }
}