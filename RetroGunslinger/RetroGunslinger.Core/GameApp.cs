using System.Numerics;
using RetroGunslinger.Core.UI.ViewModels;
using Ssit.CrossX;
using Ssit.CrossX.Core;
using Ssit.CrossX.Games.Template;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.IO;
using Ssit.CrossX.IoC;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Services;

namespace RetroGunslinger.Core;

public class GameApp: UiPixelApp
{
    protected override RgbaColor BackgroundColor => _paletteSource.Palette[1];

    private IPaletteSource _paletteSource;
    private readonly IGameTemplate _gameTemplate = new GameTemplate();

    protected override void OnInitializeServices(IIoCContainerBuilder builder)
    {
        base.OnInitializeServices(builder);
        
        var filesProvider = _gameTemplate.AssetsProvider;

        builder
            .WithInstance(filesProvider)
            .WithSingleton<ISettingsProvider, SettingsProvider>()
            .WithInstance<IFileStorage>(new FilesStorage("Gunslinger"))
            .WithInstance(_gameTemplate);
        
        builder.WithIndexedRenderer(RgbaColor.Transparent, RgbaColor.Black, RgbaColor.White, RgbaColor.Gray, RgbaColor.Red);
    }

    protected override void OnInitialize(IIoCContainer container)
    {
        _paletteSource = container.Get<IPaletteSource>();

        container
            .InitializeInputMapping()
            .InitializeFonts();
            //.InitializeGame()
            //.InitializeMusic("Menu");
        
        base.OnInitialize(container);
        UiApp.Initialize<MainPageViewModel>();
    }

    protected override void OnInitializeUi(IIoCContainerBuilder builder, INavigationMap navigationMap, IHandlerMapper handlers)
    {
        base.OnInitializeUi(builder, navigationMap, handlers);
        
        navigationMap.InitializeNavigation();
        handlers.InitializeCustomViews();
    }
    
    protected override IAppHost CreateAppHost(IIoCContainer container)
    {
        return container.IoCConstruct<PixelAppHost>(new PixelAppHost.Parameters
        {
            DesignSize = _gameTemplate.TargetSize,
            MinScale = 2,
            MaxScale = 2,
            Mode = PixelAppHost.Mode.Height,
            GlowParameters = new PixelAppHost.GlowParameters
            {
                SelfGlowFactor = 0.3f,
                EnableGameGlow = false,
                Blur = Blurs.OptimizedGaussian5X5,
                BlurDivider = Blurs.Gaussian5X5Divider,
                DisplacementFactorB = new Vector2(0.0f,0.5f),
                DisplacementFactorG = new Vector2(-0.5f,0.0f),
                DisplacementFactorR = new Vector2(0.0f,-0.5f)
            },
            CrtParameters = new PixelAppHost.CrtParameters
            {
                Interline = 0.3f,
                //DisplacementFactorG = new Vector2(-0.25f,0.0f),
                //DisplacementFactorR = new Vector2(0.25f, 0)
            }
        });
    }
}   