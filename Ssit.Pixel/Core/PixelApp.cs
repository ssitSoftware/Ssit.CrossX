using System;
using Ssit.Pixel.IoC;

namespace Ssit.Pixel.Core;

public interface IApp: IDisposable
{
    void InitializeServices(IIoCContainerBuilder builder);
    void SetActive(bool active);
    void Update(float dt);
    void Draw();
    void Resize(Size size);
    void Start(object args);
    void Initialize(IIoCContainer container);
}

public abstract class PixelApp: IApp
{
    public WindowParameters WindowParameters { get; private set; }
    
    internal bool ShouldContinue { get; private set; } = true;
    
    void IApp.InitializeServices(IIoCContainerBuilder builder) => OnInitializeServices(builder);
    
    void IApp.Start(object args) => OnStart(args);
    void IApp.Initialize(IIoCContainer container) => OnInitialize(container);

    protected virtual void OnInitialize(IIoCContainer container)
    {
        WindowParameters = container.Get<WindowParameters>();
    }

    void IApp.SetActive(bool active) => OnActivate(active);

    public void Dispose()
    {
        OnDispose(true);
    }

    protected virtual void OnActivate(bool active)
    {
    }
    
    protected void Finish() => ShouldContinue = false;

    protected PixelApp()
    {
    }
    
    ~PixelApp()
    {
        OnDispose(false);
    }

    protected virtual void OnInitializeServices(IIoCContainerBuilder builder)
    {
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