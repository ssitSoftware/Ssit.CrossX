using SampleGame.Game.UI.Pages;
using SampleGame.Game.UI.Templates;
using SampleGame.Game.UI.ViewModels;
using Ssit.CrossX;
using Ssit.CrossX.Core;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Input;
using Ssit.CrossX.IO;
using Ssit.CrossX.IoC;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Services;
using RectangleF = Ssit.CrossX.RectangleF;
using Size = Ssit.CrossX.Size;

namespace SampleGame.Game;

public class GameApp: PixelApp
{
    private IRenderer _renderer;
    private IUiApp _uiApp;
    //private IPointingDevices _pointingDevices;
    
    protected override void OnInitializeServices(IIoCContainerBuilder builder)
    {
        var bundleProvider = new BundleFilesProvider();
        var embeddedProvider = new EmbeddedFilesProvider(typeof(GameApp).Assembly, "SampleGame.Assets");

        var filesProvider = new AggregatedFilesProvider();
        filesProvider.AddProvider("assets:", embeddedProvider);
        filesProvider.AddProvider("bundle:", bundleProvider);

        builder
            .WithInstance<IFilesProvider>(filesProvider);
    }

    private void OnInitializeUi(INavigationMap navigationMap, IIoCContainerBuilder builder)
    {
        navigationMap
            .Map<MainPageViewModel, MainPage>()
            .Map<GamePageViewModel, GamePage>();

        builder
            .WithInstance(new ItemTemplates());
    }
    
    protected override void OnDispose(bool disposing)
    {
        base.OnDispose(disposing);
        _uiApp.Dispose();
    }
    
    private void MapHandlers(IHandlerMapper _)
    {
    }

    protected override void OnInitialize(IIoCContainer container)
    {
        base.OnInitialize(container);
        
        _renderer = container.Get<IRenderingWindow>().Renderer;
        
        var inputMappings = container.Get<IInputMappings>();
        var mapper = inputMappings.Mapper(0);

        mapper.MapAxis("Horizontal", GameControllerAxis.LeftX);
        mapper.MapAxis("Vertical", GameControllerAxis.LeftY);
        mapper.MapAxis("Horizontal", GameControllerButton.DPadLeft, GameControllerButton.DPadRight);
        mapper.MapAxis("Vertical", GameControllerButton.DPadUp, GameControllerButton.DPadDown);
        mapper.MapAxis("Horizontal", Key.Left, Key.Right);
        mapper.MapAxis("Vertical", Key.Up, Key.Down);
        
        mapper.MapButton("Fire", GameControllerButton.X);
        mapper.MapButton("Fire", Key.X);
        
        var fontsManager = container.Get<IFontsManager>();
        fontsManager.LoadFonts("assets:/Fonts/Fonts.json");

        _uiApp = container.InitializeUi(OnInitializeUi);

        MapHandlers(_uiApp.Services.Get<IHandlerMapper>());
        
        _uiApp.SetBounds(new RectangleF(0, 0, _renderer.TargetSize.Width, _renderer.TargetSize.Height));
        _uiApp.Navigation.NavigateTo<MainPageViewModel>();
        
        //_pointingDevices = container.Get<IPointingDevices>();
    }

    protected override void OnUpdate(float elapsedTime) => _uiApp.Update(elapsedTime);

    protected override void OnDraw()
    {
        _uiApp.Draw(_renderer, RgbaColor.Black);
        // foreach (var ptr in _pointingDevices.Pointers)
        // {
        //     _renderer.FillRectangle(new RectangleF(ptr.Position - new Vector2(5,5), new SizeF(10,10)), RgbaColor.Red);
        // }
    }
    protected override void OnResize(Size size) => _uiApp.SetBounds(new RectangleF(0, 0, size.Width, size.Height));
}