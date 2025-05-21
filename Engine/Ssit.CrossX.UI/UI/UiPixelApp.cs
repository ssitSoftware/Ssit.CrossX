using System;
using System.Diagnostics;
using System.Numerics;
using Ssit.CrossX.Common;
using Ssit.CrossX.Core;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.IoC;
using Ssit.CrossX.UI.Services;

namespace Ssit.CrossX.UI;

public abstract class UiPixelApp : IApp
{
    void IDisposable.Dispose()
    {
        OnDispose(true);
        GC.SuppressFinalize(this);
    }
    
    void IApp.Initialize( IIoCContainer container ) => OnInitialize(container);
    void IApp.SetActive(bool active) => IsActive = active;
    void IApp.Update( float dt ) => OnUpdate(dt);
    void IApp.Draw(IRenderer2 renderer) => OnDraw(renderer);
    void IApp.Resize( Size size ) => OnResize(size);
    void IApp.Start(object args) => OnStart(args);
    void IApp.InitializeServices(IIoCContainerBuilder builder) => OnInitializeServices(builder);

    protected IUiApp UiApp { get; private set; }
    public bool IsActive { get; private set; }
    
    private IAppHost _appHost;
    private IRenderer2 _renderer;

    private Size _size;
        
    protected abstract RgbaColor BackgroundColor { get; }
    
    protected virtual void OnDispose(bool disposing)
    {
        if (disposing)
        {
            UiApp?.Dispose();
            UiApp = null;
                
            _appHost?.Dispose();
            _appHost = null;
        }
    }

    protected virtual void OnInitialize(IIoCContainer container)
    {
        _renderer = container.Get<IRenderer2>();
        _appHost = CreateAppHost(container);
            
        UiApp = container.InitializeUi(OnInitializeUi);
        OnResize(_renderer.TargetSize);
    }

    protected virtual void OnInitializeUi(IIoCContainerBuilder builder, INavigationMap navigationMap, IHandlerMapper handlers)
    {
        builder
            .WithInstance<IInputCoordinateSystem>(new InputCoordinateSystem(_appHost))
            .WithInstance(_appHost)
            .WithCommonUi();

        handlers.AddCommonUiMaping();
    }
    
    protected virtual void OnInitializeServices(IIoCContainerBuilder builder)
    {
    }
        
    protected void OnResize(Size size)
    {
        _size = size;
        _appHost.Resize(_size);
        UiApp.SetBounds(new RectangleF(Vector2.Zero, _appHost.TargetSize / _appHost.Scale), _appHost.Scale);
    }

    protected void ApplyHostParameters()
    {
        _appHost?.Resize(_size, true);
    }
        
    protected virtual void OnDraw(IRenderer2 renderer)
    {
        Debug.Assert(renderer == _renderer, "renderer is not the same as the one used in the app");
        
        _renderer.Clear(RgbaColor.Black);
        _appHost.Render(this, Render);
    }
    
    protected virtual void OnStart(object args)
    {
        
    }

    private static void Render(object state)
    {
        var app = (UiPixelApp)state;
        app.UiApp.Draw(app._renderer, app.BackgroundColor);
    }

    public virtual void OnUpdate(float elapsedTime) => UiApp.Update(elapsedTime);

    protected abstract IAppHost CreateAppHost(IIoCContainer container);
}