using Gunslinger.Core.UI.Handlers;
using Gunslinger.Core.UI.Pages;
using Gunslinger.Core.UI.ViewModels;
using Gunslinger.Core.UI.Views;
using Ssit.CrossX;
using Ssit.CrossX.Core;
using Ssit.CrossX.Games.Template;
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
                .WithInstance(_template);
        }

        protected override void OnInitialize(IIoCContainer container)
        {
            container
                .InitializeInputMapping()
                .InitializeFonts()
                .InitializeGame()
                .InitializeMusic("Menu");
            
            base.OnInitialize(container);
            
            UiApp.Initialize();
            UiApp.Navigation.NavigateTo<MainPageViewModel>();
        }

        protected override void OnInitializeUi(IIoCContainerBuilder builder, INavigationMap navigationMap, IHandlerMapper handlers)
        {
            base.OnInitializeUi(builder, navigationMap, handlers);
            
            navigationMap
                .Map<MainPageViewModel, MainPage>()
                .Map<OptionsPageViewModel, OptionsPage>()
                .Map<GamePageViewModel, GamePage>();

            handlers
                .AddMapping<LabelButtonEx, LabelButtonExHandler>();
        }

        protected override IAppHost CreateAppHost(IIoCContainer container) =>
            container.IoCConstruct<PixelAppHost>(new PixelAppHost.Parameters
            {
                DesignSize = _template.TargetSize,
                Mode = PixelAppHost.Mode.Height,
                MaxScale = 4
            });
    }
}