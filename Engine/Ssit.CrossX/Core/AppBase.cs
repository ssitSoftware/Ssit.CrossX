using System;
using System.Collections.Generic;
using System.Linq;
using Ssit.CrossX.Content;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Font;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.Input;
using Ssit.CrossX.IO;
using Ssit.IoC;

namespace Ssit.CrossX.Core;

public abstract class AppBase : IApp, IKeyboardEventHandler, IResourcesLoaderSettings
{
    private GraphicsMode _mode;

    // ReSharper disable once InconsistentNaming
    protected enum ___FullscreenMode
    {
        Fullscreen
    }

    protected const ___FullscreenMode Fullscreen = ___FullscreenMode.Fullscreen;
    
    protected readonly struct GraphicsMode(int width, int height, bool fullscreen = false)
    {
        public int Width { get; } = width;
        public int Height { get; } = height;
        public bool Windowed { get; } = !fullscreen;

        public static implicit operator GraphicsMode(___FullscreenMode _) => new(800, 600, true);
        public static implicit operator GraphicsMode(Size size) => new(size.Width, size.Height);
        public static implicit operator GraphicsMode((int w, int h) mode) => new(mode.w, mode.h);
    }
    
    private readonly IFontSource[] _fontSources;
    
    LoadTextureColorMode IResourcesLoaderSettings.DefaultColorMode => DefaultColorMode;
 
    protected virtual LoadTextureColorMode DefaultColorMode => LoadTextureColorMode.Default;

    private IAppWindowManager _windowManager;
    protected IContentManager ContentManager { get; private set; }
    protected IFontsManager FontsManager { get; private set; }
    protected IActionScheduler Scheduler { get; private set; }

    protected ISmartTextRenderer SmartTextRenderer { get; private set; }
    
    protected IKeyboard Keyboard { get; private set; }

    protected IInputMappings InputMappings { get; private set; }
    
    public bool IsActive { get; private set; }

    protected GraphicsMode Mode
    {
        get => _mode;
        set
        {
            _mode = value;
            UpdateGraphicsMode();
        }
    }

    private AppBase(GraphicsMode mode, IFontSource[] fontSources)
    {
        _mode = mode;
        _fontSources = fontSources;
    }
    
    protected AppBase(int w, int h, params IFontSource[] fontSources): this(new GraphicsMode(w, h), fontSources)
    {
    }

    protected AppBase(int w, int h, ___FullscreenMode _, params IFontSource[] fontSources) 
        : this(new(w, h, true), fontSources)
    {
    }

    void IDisposable.Dispose()
    {
        OnDispose(true);
    }

    void IApp.Resize(Size size) => OnResize(size);

    void IApp.Start(object args) => OnStart(args);

    void IApp.Initialize(IIoCContainer container)
    {
        _windowManager = container.Get<IAppWindowManager>();
        ContentManager = container.Get<IContentManager>();
        FontsManager = container.Get<IFontsManager>();
        Scheduler = container.Get<IActionScheduler>();
        Keyboard = container.Get<IKeyboard>();
        InputMappings = container.Get<IInputMappings>();
        SmartTextRenderer = container.Get<ISmartTextRenderer>();

        foreach (var src in _fontSources)
        {
            FontsManager.LoadFonts(src.FontsDriveName + "/Fonts.json");
        }

        OnInitialize(container);
        UpdateGraphicsMode();
    }

    void IKeyboardEventHandler.OnKeyDown( Key key ) => OnKeyDown(key);
    void IKeyboardEventHandler.OnKeyUp( Key key ) => OnKeyUp(key);
    
    protected virtual void OnKeyDown(Key key)
    {
        if (key == Key.F11)
        {
            Mode = new GraphicsMode(Mode.Width, Mode.Height, Mode.Windowed);
        }
    }

    protected virtual void OnKeyUp(Key key)
    {
    }

    protected int PreparePixelRendering(IRenderer2 renderer, Size targetSize, bool forceAspect = true)
    {
        var (_, scale) = PrepareNormalizedRendering(renderer, targetSize, TextureFilter.Nearest, true, true);
        return (int)MathF.Round(scale);
    }

    protected (SizeF size, float scale) PrepareNormalizedRendering(IRenderer2 renderer, Size targetSize, TextureFilter filter = TextureFilter.Linear, bool forceAspect = false, bool pixelPerfect = false)
    {
        var scale = MathF.Min((float) renderer.TargetSize.Width  / targetSize.Width, (float) renderer.TargetSize.Height / targetSize.Height);
        
        if (pixelPerfect)
        {
            scale = Math.Max(1, (int)MathF.Floor(scale));
        }

        var size = renderer.TargetSize.ToVector() / scale;
        
        if(forceAspect)
        {
            var offset = (renderer.TargetSize.ToVector() - targetSize.ToVector() * scale) / 2;
            renderer.StateManager.Translate(offset);
            renderer.StateManager.Scale(scale);
            
            size = targetSize.ToVector();
            renderer.StateManager.SetClipRect(new RectangleF(0, 0, targetSize.Width, targetSize.Height));
        }
        else
        {
            renderer.StateManager.Scale(scale);
        }
        
        renderer.StateManager.SetTextureFilter(filter);
        return (size, scale);
    }
    
    protected void Close() => _windowManager.Close();

    void IApp.InitializeServices(IIoCContainerBuilder builder)
    {
        OnInitializeServices(builder);
    }

    void IApp.SetActive(bool active)
    {
        IsActive = active;
        OnActivate(active);
    }

    void IApp.Update(float dt)
    {
        OnUpdate(dt);   
    }

    void IApp.Draw(IRenderer2 renderer)
    {
        if (renderer is null)
        {
            return;
        }
        
        renderer.StateManager.Reset();
        OnDraw(renderer);
    }
    
    protected virtual void OnInitializeServices(IIoCContainerBuilder builder)
    {
        var assembly = GetType().Assembly;
        var assetsProvider = new AggregatedFilesProvider();

        assetsProvider.AddProvider("assets:", new EmbeddedFilesProvider(assembly, assembly.GetName().Name + ".Assets"));
        assetsProvider.AddProvider("bundle:", new BundleFilesProvider());
        
        PrepareAssetDrives(assetsProvider);

        foreach (var src in _fontSources)
        {
            assetsProvider.AddProvider(src.FontsDriveName, src.FontsFilesProvider);
        }
        
        builder
            .WithInstance<IFilesProvider>(assetsProvider)
            .WithInstance<IResourcesLoaderSettings>(this);
    }

    protected virtual void PrepareAssetDrives(AggregatedFilesProvider filesProvider)
    {
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
    
    private void UpdateGraphicsMode()
    {
        if (Mode.Windowed)
        {
            _windowManager.SetWindowed(new Size(Mode.Width, Mode.Height));
        }
        else
        {
            _windowManager.SetFullscreen();
        }
    }
}