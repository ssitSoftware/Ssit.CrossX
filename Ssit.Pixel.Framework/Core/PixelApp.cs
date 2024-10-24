using System;
using Ssit.Pixel.Framework.Graphics;
using Ssit.Utils.IoC;

namespace Ssit.Pixel.Framework.Core;

public abstract class PixelApp
{
    internal bool ShouldContinue { get; private set; } = true;
    
    internal IIoCContainer InitializeServices(IIoCContainerBuilder builder, Action<IIoCContainerBuilder> postConfigure) 
        => OnInitializeServices(builder, postConfigure);
    
    internal void Start(object args) => OnStart(args);

    internal void Dispose()
    {
        OnDispose(true);
    }

    protected void Finish() => ShouldContinue = false;
    
    protected IRenderingDevice Device { get; }

    protected PixelApp(IRenderingDevice device)
    {
        Device = device;
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
        Device.Renderer.Clear(RgbaColor.CornflowerBlue);
    }
    
    protected virtual void OnDispose(bool disposing)
    {
    }
}