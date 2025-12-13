using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

    private readonly List<(int, string)> _mappedButtons = new();
    
    LoadTextureColorMode IResourcesLoaderSettings.DefaultColorMode => DefaultColorMode;
 
    protected virtual LoadTextureColorMode DefaultColorMode => LoadTextureColorMode.Default;

    private IAppWindowManager _windowManager;
    protected IContentManager ContentManager { get; private set; }
    protected IFontsManager FontsManager { get; private set; }
    protected IActionScheduler Scheduler { get; private set; }

    protected ISmartTextRenderer SmartTextRenderer { get; private set; }
    
    protected IKeyboard Keyboard { get; private set; }

    private IInputMappings _inputMappings;
    
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
    
    protected AppBase(int w, int h) => _mode = (w,h);
    protected AppBase(int w, int h, ___FullscreenMode _) => _mode = new(w,h,true);

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
        _inputMappings = container.Get<IInputMappings>();
        SmartTextRenderer = container.Get<ISmartTextRenderer>();
        
        OnInitialize(container);
        UpdateGraphicsMode();

        MapInput(_ => { });
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

    protected (SizeF size, float scale) PrepareNormalizedRendering(IRenderer2 renderer, Size targetSize, TextureFilter filter = TextureFilter.Linear)
    {
        var scale = MathF.Min((float) renderer.TargetSize.Width  / targetSize.Width, (float) renderer.TargetSize.Height / targetSize.Height);
        renderer.StateManager.Scale(scale);
        
        renderer.StateManager.SetTextureFilter(filter);
        return (renderer.TargetSize.ToVector() / scale, scale);
    }
    
    protected void Close() => _windowManager.Close();

    protected void MapInput(Action<IInputMappings> func)
    {
        func?.Invoke(_inputMappings);

        if (_inputMappings is IInputMappingsInt imi)
        {
            _mappedButtons.Clear();
            foreach (var player in imi.MappedPlayers)
            {
                if (_inputMappings.Mapper(player) is IMapperInt mapperInt)
                {
                    _mappedButtons.AddRange(mapperInt.GetMappedButtons().Select(o => (player, o)));
                }
            }
        }
    }

    protected void MapInput(int player, Action<IMapper> func)
    {
        func?.Invoke(_inputMappings.Mapper(player));
        
        if (_inputMappings.Mapper(player) is IMapperInt mapperInt)
        {
            _mappedButtons.Clear();
            _mappedButtons.AddRange(mapperInt.GetMappedButtons().Select( o=> (player, o)));
        }
    }

    protected void MapButton(int player, string name, GameControllerButton btn, GameControllerButton btnAlt = GameControllerButton.None)
    {
        _inputMappings.Mapper(player).MapButton(name, btn, btnAlt);
        if (!_mappedButtons.Contains((player, name)))
        {
            _mappedButtons.Add((player, name));
        }
    }
    
    protected void MapButton(int player, string name, Key key, Key keyAlt = Key.None)
    {
        _inputMappings.Mapper(player).MapButton(name, key, keyAlt);

        if (!_mappedButtons.Contains((player, name)))
        {
            _mappedButtons.Add((player, name));
        }
    }

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
        AnalyzeInput();
        OnUpdate(dt);   
    }

    private void AnalyzeInput()
    {
        foreach (var btn in _mappedButtons)
        {
            var state = _inputMappings[btn.Item1].GetButton(btn.Item2);
            if (state.IsChanged)
            {
                if (state.IsDown)
                {
                    OnButtonDown(btn.Item1, btn.Item2);
                }
                else
                {
                    OnButtonUp(btn.Item1, btn.Item2);
                }
            }
        }
    }

    protected virtual void OnButtonUp(int player, string button)
    {
    }

    protected virtual void OnButtonDown(int player, string button)
    {
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
        PrepareAssetDrives(assetsProvider);
        
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

    protected virtual void OnDraw([NotNull]IRenderer2 renderer)
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