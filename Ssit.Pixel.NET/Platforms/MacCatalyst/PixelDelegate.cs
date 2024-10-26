using System;
using Foundation;
using Metal;
using MetalKit;
using Ssit.Pixel.Core;
using Ssit.Pixel.Graphics;
using Ssit.Pixel.Input;
using Ssit.Pixel.IoC;
using Ssit.Pixel.NET.Core;
using Ssit.Pixel.NET.Graphics;
using Ssit.Pixel.NET.Input;
using UIKit;

namespace Ssit.Pixel.NET;

public class PixelDelegate<TApp>: UIApplicationDelegate, IMTKViewDelegate where TApp: PixelApp
{
    public override UIWindow? Window { get; set; }
    private MTKView _metalView;
    private RenderingDeviceImpl _renderingDevice;
    private KeyboardImpl _keyboard;
    private PlatformHandler _platformHandler;
    private PixelViewController _pixelViewController;
    private WindowParameters _windowParameters;
    
    private TApp _app;
    private IApp App => _app;

    private IIoCContainer _services;

    public override void OnActivated(UIApplication application) => App?.SetActive(true);
    public override void OnResignActivation(UIApplication application) => App?.SetActive(false);

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        _windowParameters = new WindowParameters();
        _windowParameters.ApplyParameters += WindowParametersOnApplyParameters;
        
        var iocBuilder = IoC.IoC.NewBuilder()
            .WithPixelCore();
        
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
            AutoresizingMask = UIViewAutoresizing.All
        };
        
        _pixelViewController = new PixelViewController();
        _pixelViewController.View!.AddSubview(_metalView);
        Window.RootViewController = _pixelViewController;
        
        _metalView.SampleCount = 1;
        _metalView.DepthStencilPixelFormat = MTLPixelFormat.Depth32Float_Stencil8;
        _metalView.ColorPixelFormat = MTLPixelFormat.BGRA8Unorm;
        _metalView.PreferredFramesPerSecond = 60;
        _metalView.ClearColor = new MTLClearColor(0.0f, 0.0f, 0.0f, 1.0f);
        _metalView.Delegate = this;

        _renderingDevice = new RenderingDeviceImpl(_metalView);

        _platformHandler = new PlatformHandler();
        _platformHandler.Initialize(iocBuilder);

        _keyboard = new KeyboardImpl();

        iocBuilder
            .WithInstance<IRenderingDevice>(_renderingDevice)
            .WithInstance(_renderingDevice.Renderer)
            .WithInstance<IKeyboard>(_keyboard);

        var services = iocBuilder.Build();
        _app = services.IoCConstruct<TApp>(_windowParameters);

        var newBuilder = IoC.IoC.NewBuilder();
        newBuilder.WithParent(services);
        
        IApp app = _app;
        _pixelViewController.SetApp(app);
        _services = app.InitializeServices(newBuilder, o => { });
        
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

    public void DrawableSizeWillChange(MTKView view, CoreGraphics.CGSize size)
    {
        _app.Resize(new Size((int) size.Width, (int) size.Height));
    }

    public void Draw(MTKView view)
    {
        _pixelViewController.UpdateKeyboard(_keyboard);
        _platformHandler.Tick(_app);
        _renderingDevice.Draw(view, _app);
    }
}