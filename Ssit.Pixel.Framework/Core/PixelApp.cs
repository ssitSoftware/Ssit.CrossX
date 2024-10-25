using System;
using Ssit.Pixel.Framework.Graphics;
using Ssit.Utils.IoC;

namespace Ssit.Pixel.Framework.Core;

public abstract class PixelApp
{
    public WindowParameters WindowParameters { get; }
    
    internal bool ShouldContinue { get; private set; } = true;
    
    internal IIoCContainer InitializeServices(IIoCContainerBuilder builder, Action<IIoCContainerBuilder> postConfigure) 
        => OnInitializeServices(builder, postConfigure);
    
    internal void Start(object args) => OnStart(args);

    internal void SetActive(bool active) => OnActivate(active);

    internal void Dispose()
    {
        OnDispose(true);
    }

    protected virtual void OnActivate(bool active)
    {
    }
    
    protected void Finish() => ShouldContinue = false;

    protected PixelApp(WindowParameters windowParameters)
    {
        WindowParameters = windowParameters;
    }
    
    ~PixelApp()
    {
        OnDispose(false);
    }

    protected virtual IIoCContainer OnInitializeServices(IIoCContainerBuilder builder,
        Action<IIoCContainerBuilder> postConfigure)
    {
        postConfigure?.Invoke(builder);
        return builder.Build();
    }

    protected virtual void OnStart(object args)
    {
    }

    internal void Update(float elapsedTime) => OnUpdate(elapsedTime);

    protected virtual void OnUpdate(float elapsedTime)
    {
    }

    internal void Draw() => OnDraw();

    protected virtual void OnDraw()
    {
    }
    
    protected virtual void OnDispose(bool disposing)
    {
    }
}