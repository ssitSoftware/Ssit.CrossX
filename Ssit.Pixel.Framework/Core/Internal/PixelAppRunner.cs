using System;
using System.Threading.Tasks;
using Ssit.Utils.IoC;

namespace Ssit.Pixel.Framework.Core.Internal;

public abstract class PixelAppRunner<TApp> where TApp: PixelApp
{
    ~PixelAppRunner()
    {
        OnDispose(false);
    }

    protected async Task Run(IIoCContainerBuilder builder = null, object args = null)
    {
        builder ??= IoC.NewBuilder();
        
        InitializePlatform(builder, args);
        
        var appData = InitializeApp(builder, PostInitialize, args);

        appData.app.Start(args);
        await Loop(appData.app);
        
        appData.app.Dispose();
        
        OnDispose(true);
        
        appData.c1.Dispose();
        appData.c0.Dispose();
    }

    protected virtual void PostInitialize(IIoCContainerBuilder builder)
    {
    }

    protected abstract Task Loop(TApp app);

    protected void UpdateApp(TApp app, float dt) => app.Update(dt);
    protected void DrawApp(TApp app) => app.Draw();

    protected abstract void InitializePlatform(IIoCContainerBuilder builder, object args);

    private (IIoCContainer c1, IIoCContainer c0, TApp app) InitializeApp(
        IIoCContainerBuilder builder, 
        Action<IIoCContainerBuilder> postConfigure,
        object args)
    {
        var container = builder.WithPixelCore().Build();
        var app = container.IoCConstruct<TApp>(args);
        
        var newBuilder = IoC.NewBuilder()
            .WithParent(container);

        var newContainer = app.InitializeServices(newBuilder, postConfigure);
        
        return (newContainer, container, app);
    }
    
    protected virtual void OnDispose(bool disposing)
    {
    }
}