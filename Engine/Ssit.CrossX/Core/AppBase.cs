using System;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.IO;
using Ssit.IoC;

namespace Ssit.CrossX.Core;

public abstract class AppBase : IApp, IResourcesLoaderSettings
{
    LoadTextureColorMode IResourcesLoaderSettings.DefaultColorMode => DefaultColorMode;
 
    protected virtual LoadTextureColorMode DefaultColorMode => LoadTextureColorMode.Default;
    
    public bool IsActive { get; private set; }
    
    void IDisposable.Dispose()
    {
        OnDispose(true);
    }

    void IApp.Resize(Size size) => OnResize(size);

    void IApp.Start(object args) => OnStart(args);
    void IApp.Initialize(IIoCContainer container) => OnInitialize(container);

    
    void IApp.InitializeServices(IIoCContainerBuilder builder)
    {
        OnInitializeServices(builder);
    }

    void IApp.SetActive(bool active)
    {
        IsActive = active;
        OnActivate(active);
    }
    
    void IApp.Update(float dt) => OnUpdate(dt);

    void IApp.Draw(IRenderer2 renderer) => OnDraw(renderer);
    
    protected virtual void OnInitializeServices(IIoCContainerBuilder builder)
    {
        var assetsProvider = new EmbeddedFilesProvider(GetType().Assembly);
        
        builder
            .WithInstance<IFilesProvider>(assetsProvider)
            .WithInstance<IResourcesLoaderSettings>(this);
    }
    
    protected virtual void OnDispose(bool dispose)
    {
    }
    
    protected virtual void OnActivate(bool active)
    {
    }
    
    protected virtual void OnUpdate(float dt)
    {
        
    }

    

    protected virtual void OnDraw(IRenderer2 renderer)
    {
        renderer.Clear(RgbaColor.CornflowerBlue);
    }

    
    
    protected virtual void OnResize(Size size)
    {
        
    }

    

    protected virtual void OnStart(object args)
    {
    }

    

    protected virtual void OnInitialize(IIoCContainer container)
    {
        
    }
}