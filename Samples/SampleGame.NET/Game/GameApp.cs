using System.Numerics;
using SampleGame.Game.Logic;
using SampleGame.Game.Rendering;
using SampleGame.Services;
using SampleGame.UI.Pages;
using SampleGame.UI.Pages.Internal;
using SampleGame.UI.Templates;
using SampleGame.UI.Views;
using Ssit.CrossX;
using Ssit.CrossX.Content;
using Ssit.CrossX.Core;
using Ssit.CrossX.Games;
using Ssit.CrossX.Games.Rendering;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Input;
using Ssit.CrossX.IO;
using Ssit.CrossX.IoC;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Services;
using RectangleF = Ssit.CrossX.RectangleF;
using Size = Ssit.CrossX.Size;

namespace SampleGame.Game;

public class GameApp: PixelApp, IInputCoordinateSystem
{
    private IRenderer _renderer;
    private IUiApp _uiApp;
    private IPointingDevices _pointingDevices;

    private ShooterPlayerBrain _playerBrain;
    private SpriteShooterRenderer _spriteShooterRenderer;

    private PixelAppHost _appHost;

    public Matrix3x2 Transform
    {
        get
        {
            if (Matrix3x2.Invert(_appHost.Transform, out var matrix))
            {
                return matrix;
            }
            return Matrix3x2.Identity;
        }
    }
    
    protected override void OnInitializeServices(IIoCContainerBuilder builder)
    {
        var bundleProvider = new BundleFilesProvider();
        var embeddedProvider = new EmbeddedFilesProvider(typeof(GameApp).Assembly, "SampleGame.Assets");

        var filesProvider = new AggregatedFilesProvider();
        filesProvider.AddProvider("assets:", embeddedProvider);
        filesProvider.AddProvider("bundle:", bundleProvider);

        builder
            .WithInstance<IFilesProvider>(filesProvider)
            .WithSingleton<IGameSettings, GameSettings>();
    }

    private void OnInitializeUi(INavigationMap navigationMap, IIoCContainerBuilder builder)
    {
        navigationMap
            .Map<MainPageViewModel, MainPage>()
            .Map<OptionsPageViewModel, OptionsPage>()
            .Map<GamePageViewModel, GamePage>();

        builder
            .WithInstance(new ItemTemplates())
            .WithInstance(new PageInputContext())
            .WithSingleton<ITranslator, Translator>()
            .WithInstance<IInputCoordinateSystem>(this);
    }
    
    protected override void OnDispose(bool disposing)
    {
        base.OnDispose(disposing);
        _uiApp.Dispose();
    }
    
    private void MapHandlers(IHandlerMapper mapper)
    {
        mapper
            .AddMapping<GameView, GameViewHandler>()
            .AddMapping<PointsView, PointsViewHandler>();
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
        mapper.MapButton("Melee", Key.C);
        
        mapper.MapButton("Roll", GameControllerButton.A);
        mapper.MapButton("Roll", Key.Space);
        
        mapper.MapButton("Reload", GameControllerButton.B);
        
        var fontsManager = container.Get<IFontsManager>();
        fontsManager.LoadFonts("assets:/Fonts/Fonts.json");

        _uiApp = container.InitializeUi(OnInitializeUi);

        MapHandlers(_uiApp.Services.Get<IHandlerMapper>());
        
        _pointingDevices = container.Get<IPointingDevices>();

        var cm = container.Get<IContentManager>();
        cm.RegisterGameContentTypes();
        
        using var shadowObj = cm.Get<GameObject>("assets:/Sprites/CharacterShadow");
        using var heroObj = cm.Get<GameObject>("assets:/Sprites/Hero");
        using var gunObj = cm.Get<GameObject>("assets:/Sprites/HeroGun");
        
        _spriteShooterRenderer = new SpriteShooterRenderer(heroObj, gunObj, new Vector2(0,-9), shadowObj);
        _playerBrain = new ShooterPlayerBrain(new PlayerController(inputMappings), _spriteShooterRenderer);

        _appHost = container.IoCConstruct<PixelAppHost>(new PixelAppHost.Parameters
        {
            DesignSize = new Size(480, 360),
            Mode = PixelAppHost.Mode.Height,
            Renderer = _renderer,
            MaxScale = 2
        });
        
        OnResize(_renderer.TargetSize);
        _uiApp.Navigation.NavigateTo<MainPageViewModel>();
    }

    protected override void OnUpdate(float elapsedTime)
    {
        _playerBrain.Update(elapsedTime);
        _uiApp.Update(elapsedTime);
    }

    protected override void OnDraw()
    {
        _renderer.Clear(RgbaColor.Black);
        _appHost.Render( Render );
    }

    private void Render()
    {
        _renderer.Clear(RgbaColor.FromBgra(0xff513a3d));
        _uiApp.Draw(_renderer);

        _spriteShooterRenderer.Scale = _appHost.Scale;

        _spriteShooterRenderer.Render(_renderer, _playerBrain.Position + _appHost.DesignTargetSize.ToVector() / 2,
            RenderPass.Shadow);
        _spriteShooterRenderer.Render(_renderer, _playerBrain.Position + _appHost.DesignTargetSize.ToVector() / 2,
            RenderPass.Normal);
    }

    protected override void OnResize(Size size)
    {
        _appHost.Resize(size);
        _uiApp.SetBounds(new RectangleF(Vector2.Zero, _appHost.TargetSize / _appHost.Scale), _appHost.Scale);
    }
}