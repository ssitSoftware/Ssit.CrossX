using System;
using System.Diagnostics;
using Ssit.CrossX.IoC;

namespace Ssit.CrossX.Core;

public abstract class PixelApp: IApp
{
    public WindowParameters WindowParameters { get; private set; }
    
    public bool IsActive { get; private set; }
    
    private readonly Stopwatch _stopwatch = new();
    private TimeSpan _lastTime;
    
    internal bool ShouldContinue { get; private set; } = true;
    
    void IApp.InitializeServices(IIoCContainerBuilder builder) => OnInitializeServices(builder);

    void IApp.Update(float dt) => OnUpdate(dt);
    
    void IApp.Start(object args)
    {
        OnStart(args);
        
        _stopwatch.Start();
        _lastTime = _stopwatch.Elapsed;
    }
    
    void IApp.Initialize(IIoCContainer container) => OnInitialize(container);

    protected virtual void OnInitialize(IIoCContainer container)
    {
        WindowParameters = container.Get<WindowParameters>();
    }

    void IApp.SetActive(bool active)
    {
        IsActive = active;
        OnActivate(active);
    }

    public void Dispose()
    {
        OnDispose(true);
        _stopwatch.Stop();
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

    void IApp.Update(Action<float> preUpdate)
    {
        var stopwatchRead = _stopwatch.Elapsed;
        var dt = (float)(stopwatchRead - _lastTime).TotalSeconds;
        _lastTime = stopwatchRead;

        preUpdate(dt);
        OnUpdate(dt);
    }

    public virtual void OnUpdate(float elapsedTime)
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