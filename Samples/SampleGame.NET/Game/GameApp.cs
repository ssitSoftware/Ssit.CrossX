using System.Numerics;
using SampleGame.Game.Logic;
using SampleGame.Game.Rendering;
using SampleGame.Game.UI.Pages;
using SampleGame.Game.UI.Templates;
using SampleGame.Game.UI.ViewModels;
using Ssit.CrossX;
using Ssit.CrossX.Content;
using Ssit.CrossX.Core;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Sprites;
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
    private IPointingDevices _pointingDevices;

    private SpriteInstance _spriteInstance;
    private SpriteInstance _spriteGunInstance;

    private ShooterPlayerBrain _playerBrain;
    private SpriteShooterRenderer _spriteShooterRenderer;
    
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
        
        _spriteInstance?.Dispose();
        _spriteGunInstance?.Dispose();
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
        mapper.MapAxis("Horizontal", Key.A, Key.D);
        mapper.MapAxis("Vertical", Key.W, Key.S);
        
        mapper.MapAxis("AimX", GameControllerAxis.RightX);
        mapper.MapAxis("AimY", GameControllerAxis.RightY);
        
        mapper.MapButton("Shoot", GameControllerButton.RightShoulder);
        mapper.MapButton("Shoot", Key.X);
        
        mapper.MapButton("Melee", GameControllerButton.X);
        
        mapper.MapButton("Roll", GameControllerButton.A);
        mapper.MapButton("Reload", GameControllerButton.B);
        
        var fontsManager = container.Get<IFontsManager>();
        fontsManager.LoadFonts("assets:/Fonts/Fonts.json");

        _uiApp = container.InitializeUi(OnInitializeUi);

        MapHandlers(_uiApp.Services.Get<IHandlerMapper>());
        
        _uiApp.SetBounds(new RectangleF(0, 0, _renderer.TargetSize.Width, _renderer.TargetSize.Height));
        _uiApp.Navigation.NavigateTo<MainPageViewModel>();
        
        _pointingDevices = container.Get<IPointingDevices>();

        var cm = container.Get<IContentManager>();
        _spriteInstance = new SpriteInstance("assets:/Sprites/Hero.json", new Vector2(93, 96), cm);
        _spriteGunInstance = new SpriteInstance("assets:/Sprites/HeroGun.json", new Vector2(16, 16), cm);
        _spriteShooterRenderer = new SpriteShooterRenderer(_spriteInstance, _spriteGunInstance, new Vector2(0,-8));
        _playerBrain = new ShooterPlayerBrain(new PlayerController(inputMappings), _spriteShooterRenderer);
    }

    protected override void OnUpdate(float elapsedTime)
    {
        _playerBrain.Update(elapsedTime);
    }

    protected override void OnDraw()
    {
        _renderer.Clear(RgbaColor.SaddleBrown);
        
        foreach (var ptr in _pointingDevices.Pointers)
        {
            _renderer.FillRectangle(new RectangleF(ptr.Position - new Vector2(5,5), new SizeF(10,10)), RgbaColor.Red);
        }

        _spriteShooterRenderer.Scale = 4;
        _spriteShooterRenderer.Render(_renderer, _playerBrain.Position + _renderer.TargetSize.ToVector() / 8);
    }

    protected override void OnResize(Size size) => _uiApp.SetBounds(new RectangleF(0, 0, size.Width, size.Height));
}