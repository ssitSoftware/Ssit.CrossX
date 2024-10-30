using System;
using Foundation;
using Metal;
using MetalKit;
using Microsoft.Maui.Controls;
using Ssit.Pixel.Audio;
using Ssit.Pixel.Core;
using Ssit.Pixel.Graphics;
using Ssit.Pixel.Input;
using Ssit.Pixel.IO;
using Ssit.Pixel.IoC;
using Ssit.Pixel.NET.Audio;
using Ssit.Pixel.NET.Core;
using Ssit.Pixel.NET.Graphics;
using Ssit.Pixel.NET.Input;
using UIKit;

namespace Ssit.Pixel.NET;

public class PixelDelegate<TApp>: UIApplicationDelegate, IMTKViewDelegate, IEventSource where TApp: PixelApp, new()
{
    public override UIWindow Window { get; set; }
    
    public event Action<float> Updating;
    public event Action Updated;
    public event Action RenderFinished;
    
    private MTKView _metalView;
    private RenderingWindowImpl _renderingWindow;
    private KeyboardImpl _keyboard;
    private PlatformHandler _platformHandler;
    private PixelViewController _pixelViewController;
    private WindowParameters _windowParameters;
    
    private TApp _app;
    private IApp App => _app;

    private IIoCContainer _services;
    private bool _isDisposed;

    private bool _isActive = true;
    
    public override void OnActivated(UIApplication application) => App?.SetActive(_isActive = true);
    public override void OnResignActivation(UIApplication application) => App?.SetActive(_isActive = false);

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        _windowParameters = new WindowParameters();
        _windowParameters.ApplyParameters += WindowParametersOnApplyParameters;

        var iocBuilder = IoC.IoC.NewBuilder();
        
        Window = new UIWindow(UIScreen.MainScreen.Bounds);
        Window.WindowScene.Titlebar.TitleVisibility = UITitlebarTitleVisibility.Hidden;
        Window.WindowScene.Titlebar.Toolbar = null;
        
        if (MTLDevice.SystemDefault is  null)
        {
            Console.WriteLine("Metal is not supported on this device");
            return false;
        }
        
        _metalView = new MTKView(Window!.Frame, MTLDevice.SystemDefault)
        {
            AutoresizingMask = UIViewAutoresizing.All,
            SampleCount = 1,
            DepthStencilPixelFormat = MTLPixelFormat.Depth32Float_Stencil8,
            ColorPixelFormat = MTLPixelFormat.BGRA8Unorm,
            PreferredFramesPerSecond = 60,
            ClearColor = new MTLClearColor(0.0f, 0.0f, 0.0f, 1.0f),
            Delegate = this
        };
        
        _pixelViewController = new PixelViewController();
        _pixelViewController.View!.AddSubview(_metalView);
        Window.RootViewController = _pixelViewController;

        _renderingWindow = new RenderingWindowImpl(_metalView);

        _platformHandler = new PlatformHandler();
        _platformHandler.Initialize(iocBuilder);

        _keyboard = new KeyboardImpl();
        
        iocBuilder
            .WithInstance<IRenderingWindow>(_renderingWindow)
            .WithInstance<IMetalDevice>(_renderingWindow)
            .WithInstance<IKeyboard>(_keyboard)
            .WithInstance(_metalView.Device)
            .WithInstance<IEventSource>(this)
            .WithSingleton<MTKTextureLoader, MTKTextureLoader>()
            .WithImplementation<ITexture, TextureImpl>()
            .WithImplementation<IRenderTarget, RenderTargetImpl>()
            .WithInstance(_windowParameters)
            .WithImplementation<ISoundEffect, SoundEffectImpl>()
            .WithPixelCore();
        
        _app = new TApp();
        
        IApp app = _app;
        _pixelViewController.SetApp(app);
        app.InitializeServices(iocBuilder);

        _services = iocBuilder.Build();
        
        app.Initialize(_services);
        app.Start(null);
        
        _app.Resize(new Size((int) _metalView.Layer.Bounds.Width.Value, (int) _metalView.Layer.Bounds.Height.Value));
        
        Window.MakeKeyAndVisible();
        
        _windowParameters.Width = (int)Window!.Frame.Width;
        _windowParameters.Height = (int)Window!.Frame.Height;
        
        _windowParameters.Apply(false);
        
        return true;
    }

    private void WindowParametersOnApplyParameters()
    {
    }

    public override void WillTerminate(UIApplication application)
    {
        Dispose(true);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && !_isDisposed)
        {
            _app?.Dispose();
            _app = null;

            _services?.Dispose();
            _services = null;
            
            _isDisposed = true;
        }
    }

    public void DrawableSizeWillChange(MTKView view, CoreGraphics.CGSize size)
    {
        if (_isDisposed)
        {
            return;
        }
        _app.Resize(new Size((int) size.Width, (int) size.Height));
    }

    public void Draw(MTKView view)
    {
        if (_isDisposed)
        {
            return;
        }
        
        _pixelViewController.UpdateKeyboard(_keyboard);
        _platformHandler.Tick(_app, f => Updating?.Invoke(f));
        
        Updated?.Invoke();
        
        _renderingWindow.Draw(view, _app);
        RenderFinished?.Invoke();
    }

    
}