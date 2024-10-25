using System;
using Ssit.Pixel.IoC;

namespace Ssit.Pixel.Core;

public interface IApp
{
    IIoCContainer InitializeServices(IIoCContainerBuilder builder, Action<IIoCContainerBuilder> postConfigure);
    void SetActive(bool active);
    void Update(float dt);
    void Draw();
    void Resize(Size size);
    void Start(object args);
}

public abstract class PixelApp: IApp
{
    public WindowParameters WindowParameters { get; }
    
    internal bool ShouldContinue { get; private set; } = true;
    
    IIoCContainer IApp.InitializeServices(IIoCContainerBuilder builder, Action<IIoCContainerBuilder> postConfigure) 
        => OnInitializeServices(builder, postConfigure);
    
    void IApp.Start(object args) => OnStart(args);

    void IApp.SetActive(bool active) => OnActivate(active);

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

    void IApp.Update(float elapsedTime) => OnUpdate(elapsedTime);

    protected virtual void OnUpdate(float elapsedTime)
    {
    }

    void IApp.Draw() => OnDraw();
    public void Resize(Size size) => OnResize(size);

    protected virtual void OnResize(Size size)
    {
        
    }

    protected virtual void OnDraw()
    {
    }
    
    protected virtual void OnDispose(bool disposing)
    {
    }
}