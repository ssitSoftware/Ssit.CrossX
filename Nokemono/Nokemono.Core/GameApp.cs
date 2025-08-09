using System.Numerics;
using Nokemono.Core.Configuration;
using Nokemono.Core.UI.ViewModels;
using Ssit.CrossX;
using Ssit.CrossX.Core;
using Ssit.CrossX.Games.Template;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.IO;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Services;
using Ssit.IoC;

namespace Nokemono.Core;

public class GameApp : UiPixelApp
{
    protected override RgbaColor BackgroundColor => _paletteSource.Palette[1];

    private IPaletteSource _paletteSource;
    private readonly IGameTemplate _gameTemplate = new GameTemplate();
    private readonly PixelAppHost.Parameters _hostParameters;

    public GameApp()
    {
        _hostParameters
            = new PixelAppHost.Parameters
            {
                DesignSize = _gameTemplate.TargetSize,
                MinScale = 2,
                MaxScale = 2,
                Mode = PixelAppHost.Mode.WidthAndHeightKeepAspect,
            };
    }

    protected override void OnInitializeServices(IIoCContainerBuilder builder)
    {
        base.OnInitializeServices(builder);

        var filesProvider = _gameTemplate.AssetsProvider;

        builder
            .WithInstance(filesProvider)
            .WithSingleton<ISettingsProvider, SettingsProvider>()
            .WithInstance<IFileStorage>(new FilesStorage("Gunslinger"))
            .WithInstance(_gameTemplate)
            .WithSingleton<Config, Config>();

        builder.WithIndexedRenderer(RgbaColor.Transparent, 0x000000, 0xffffff, 0x606060, 0xff0000, 0xff00ff);
    }

    protected override void OnInitialize(IIoCContainer container)
    {
        _paletteSource = container.Get<IPaletteSource>();

        var settings = container.Get<ISettingsProvider>().Settings;
        var scheduler = container.Get<IActionScheduler>();

        SetCrtMode(settings.CrtMode);
        settings.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(Settings.CrtMode))
            {
                scheduler.Schedule(() =>
                    SetCrtMode(container.Get<ISettingsProvider>().Settings.CrtMode));
            }
        };

        container
            .InitializeInputMapping()
            .InitializeFonts()
            .InitializeGame()
            .InitializeMusic("Menu");

        base.OnInitialize(container);
        UiApp.Initialize<StartupPageViewModel>();
    }

    private void SetCrtMode(int mode)
    {
        switch (mode)
        {
            case 0:
                _hostParameters.GlowParameters = new PixelAppHost.GlowParameters
                {
                    SelfGlowFactor = 0.4f,
                    EnableGameGlow = false,
                    Blur = Blurs.Gaussian3X3,
                    BlurDivider = Blurs.Gaussian3X3Divider
                };
                _hostParameters.CrtParameters = null;
                _hostParameters.Mode = PixelAppHost.Mode.Height; 
                ApplyHostParameters();
                break;

            case 1:
                SetBasicCrt();
                _hostParameters.GlowParameters.SelfGlowFactor = 0.75f;
                _hostParameters.CrtParameters.Distortion = 1.03f;
                
                _hostParameters.Mode = PixelAppHost.Mode.Height;
                
                ApplyHostParameters();
                break;

            case 2:
            {
                SetBasicCrt();
                var scale = 0.75f;

                _hostParameters.GlowParameters.DisplacementFactorR = new Vector2(-0.5f, 0.0f) * scale;
                _hostParameters.GlowParameters.DisplacementFactorG = new Vector2(0.0f, 0.0f) * scale;
                _hostParameters.GlowParameters.DisplacementFactorB = new Vector2(0.5f, 0.0f) * scale;
                _hostParameters.GlowParameters.SelfGlowFactor = 0.8f;

                _hostParameters.CrtParameters.DisplacementFactorR = new Vector2(-0.5f, 0.0f) * scale;
                _hostParameters.CrtParameters.DisplacementFactorG = new Vector2(0.0f, 0.0f) * scale;
                _hostParameters.CrtParameters.DisplacementFactorB = new Vector2(0.5f, 0.0f) * scale;
                _hostParameters.CrtParameters.LampGlow = 0.4f;
                _hostParameters.CrtParameters.LampDownSize = 4;
                _hostParameters.CrtParameters.Interline = 0.45f;
                _hostParameters.CrtParameters.Distortion = 1.075f;
                _hostParameters.CrtParameters.Vignette = 0.5f;
                _hostParameters.CrtParameters.VignettePower = 1.5f;
                
                _hostParameters.Mode = PixelAppHost.Mode.Height;
                
                ApplyHostParameters();
            }
                break;
            
            case 3:
            {
                SetBasicCrt();
                var scale = 1.0f;

                _hostParameters.GlowParameters.DisplacementFactorR = new Vector2(-0.5f, 0.0f) * scale;
                _hostParameters.GlowParameters.DisplacementFactorG = new Vector2(0.0f, 0.0f) * scale;
                _hostParameters.GlowParameters.DisplacementFactorB = new Vector2(0.75f, 0.0f) * scale;
                _hostParameters.GlowParameters.SelfGlowFactor = 0.8f;

                _hostParameters.CrtParameters.DisplacementFactorR = new Vector2(-0.5f, 0.0f) * scale;
                _hostParameters.CrtParameters.DisplacementFactorG = new Vector2(0.0f, 0.0f) * scale;
                _hostParameters.CrtParameters.DisplacementFactorB = new Vector2(0.75f, 0.0f) * scale;
                _hostParameters.CrtParameters.LampGlow = 0.8f;
                _hostParameters.CrtParameters.LampDownSize = 4;
                _hostParameters.CrtParameters.Interline = 0.5f;
                _hostParameters.CrtParameters.Distortion = 1.12f;
                _hostParameters.CrtParameters.Vignette = 1f;
                _hostParameters.CrtParameters.VignettePower = 2f;
                
                _hostParameters.Mode = PixelAppHost.Mode.Height;
                
                ApplyHostParameters();
            }
                break;
        }
    }

    private void SetBasicCrt()
    {
        _hostParameters.GlowParameters = new PixelAppHost.GlowParameters
        {
            SelfGlowFactor = 0.35f,
            EnableGameGlow = false,
            Blur = Blurs.Gaussian5X5,
            BlurDivider = Blurs.Gaussian5X5Divider
        };
        _hostParameters.CrtParameters = new PixelAppHost.CrtParameters
        {
            Interline = 0.25f,
        };
    }

    protected override void OnInitializeUi(IIoCContainerBuilder builder, INavigationMap navigationMap,
        IHandlerMapper handlers)
    {
        base.OnInitializeUi(builder, navigationMap, handlers);

        navigationMap.InitializeNavigation();
        handlers.InitializeCustomViews();
    }

    protected override IAppHost CreateAppHost(IIoCContainer container)
    {
        return container.IoCConstruct<PixelAppHost>(_hostParameters);
    }
}