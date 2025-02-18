using System.Numerics;
using Gunslinger.Core.UI.ViewModels;
using Ssit.CrossX;
using Ssit.CrossX.Core;
using Ssit.CrossX.Games.Template;
using Ssit.CrossX.IO;
using Ssit.CrossX.IoC;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Services;

namespace Gunslinger.Core
{
    public class GameApp : UiPixelApp
    {
        protected override RgbaColor BackgroundColor { get; } = RgbaColor.FromBgra(0xff101010);
        
        private readonly IGameTemplate _template = new GunslingerTemplate();
        
        protected override void OnInitializeServices(IIoCContainerBuilder builder)
        {
            base.OnInitializeServices(builder);
            
            var filesProvider = _template.AssetsProvider;

            builder
                .WithInstance(filesProvider)
                .WithSingleton<ISettingsProvider, SettingsProvider>()
                .WithInstance<IFileStorage>(new FilesStorage("Gunslinger"))
                .WithInstance(_template);
        }
        
        protected override void OnInitializeUi(IIoCContainerBuilder builder, INavigationMap navigationMap, IHandlerMapper handlers)
        {
            base.OnInitializeUi(builder, navigationMap, handlers);
            
            navigationMap.InitializeNavigation();
            handlers.InitializeCustomViews();
        }

        protected override void OnInitialize(IIoCContainer container)
        {
            container
                .InitializeInputMapping()
                .InitializeFonts()
                .InitializeGame()
                .InitializeMusic("Menu");
            
            base.OnInitialize(container);
            UiApp.Initialize<MainPageViewModel>();
        }

        protected override IAppHost CreateAppHost(IIoCContainer container) =>
            container.IoCConstruct<PixelAppHost>(new PixelAppHost.Parameters
            {
                DesignSize = _template.TargetSize,
                Mode = PixelAppHost.Mode.Height,
                MaxScale = 8,
                GlowParameters = new PixelAppHost.GlowParameters
                {
                    Blur = Blurs.Gaussian5X5,
                    BlurDivider = Blurs.Gaussian5X5Divider,
                    SelfGlowFactor = 0.25f,
                    SelfGlowDisplacementFactorB = new Vector2(1, -1),
                    SelfGlowDisplacementFactorR = new Vector2(-1, 0)
                }
            });
    }
}