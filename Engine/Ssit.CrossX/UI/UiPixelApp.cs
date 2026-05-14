using System;
using System.Diagnostics;
using System.Numerics;
using Ssit.CrossX.Core;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.UI.Common;
using Ssit.CrossX.UI.Common.Pages;
using Ssit.IoC;
using Ssit.CrossX.UI.Services;

namespace Ssit.CrossX.UI;

public abstract class UiPixelApp : IApp
{
    public virtual bool IsPortrait => false;

    void IDisposable.Dispose()
    {
        OnDispose(true);
        GC.SuppressFinalize(this);
    }
    
    void IApp.Initialize(IIoCContainer container) => OnInitialize(container);

    void IApp.SetActive(bool active) => IsActive = active;

    void IApp.Update( float dt ) => OnUpdate(dt);
    void IApp.Draw(IRenderer2 renderer) => OnDraw(renderer);
    void IApp.Resize( Size size ) => OnResize(size);
    void IApp.Start(object args) => OnStart(args);
    void IApp.InitializeServices(IIoCContainerBuilder builder) => OnInitializeServices(builder);

    protected IUiApp UiApp { get; private set; }
    public bool IsActive { get; private set; }
    
    protected IAppHost AppHost { get; private set; }
    private IRenderer2 _renderer;

    private SizeF _size;
        
    protected abstract RgbaColor BackgroundColor { get; }
    
    protected virtual void OnPaused()
    {
    }

    protected virtual void OnResumed()
    {
    }
    
    protected virtual void OnDispose(bool disposing)
    {
        if (disposing)
        {
            UiApp?.Dispose();
            UiApp = null;

            AppHost?.Dispose();
            AppHost = null;
        }
    }

    protected virtual void OnInitialize(IIoCContainer container)
    {
        _renderer = container.Get<IRenderer2>();
        AppHost = CreateAppHost(container);
            
        UiApp = container.InitializeUi(OnInitializeUi);
        OnResize(_renderer.Bounds.Size);
    }

    protected virtual void OnInitializeUi(IIoCContainerBuilder builder, INavigationMap navigationMap, IHandlerMapper handlers)
    {
        builder
            .WithInstance<IInputCoordinateSystem>(new InputCoordinateSystem(AppHost))
            .WithInstance(AppHost)
            .WithSingleton<PageInputContext, PageInputContext>();

        handlers.AddCommonUiMaping();
    }
    
    protected virtual void OnInitializeServices(IIoCContainerBuilder builder)
    {
    }
        
    protected void OnResize(SizeF size)
    {
        _size = size;
        AppHost.Resize(_size);
        UiApp.SetBounds(new RectangleF(Vector2.Zero, AppHost.TargetSize / AppHost.Scale), AppHost.Scale);
    }

    protected void ApplyHostParameters()
    {
        if (AppHost is null)
            return;
        
        AppHost.Resize(_size, true);
        UiApp.SetBounds(new RectangleF(Vector2.Zero, AppHost.TargetSize / AppHost.Scale), AppHost.Scale);
    }

    protected virtual void OnDraw(IRenderer2 renderer)
    {
        Debug.Assert(renderer == _renderer, "renderer is not the same as the one used in the app");
        
        _renderer.Clear(RgbaColor.Black);
        AppHost.Render(this, Render);
    }
    
    protected virtual void OnStart(object args)
    {
    }

    protected virtual void PostRender(IRenderer2 renderer)
    {
    }

    private static void Render(object state)
    {
        var app = (UiPixelApp)state;

        if (!app.IsActive)
            return;
        
        app.UiApp.Draw(app._renderer, app.BackgroundColor);
        app.PostRender(app._renderer);
    }

    public virtual void OnUpdate(float elapsedTime) => UiApp.Update(elapsedTime);

    protected abstract IAppHost CreateAppHost(IIoCContainer container);
}