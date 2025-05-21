using RetroGunslinger.Core.UI.Handlers;
using RetroGunslinger.Core.UI.Pages;
using RetroGunslinger.Core.UI.ViewModels;
using RetroGunslinger.Core.UI.Views;
using Ssit.CrossX.Audio;
using Ssit.CrossX.Common.Pages;
using Ssit.CrossX.Common.Services;
using Ssit.CrossX.Content;
using Ssit.CrossX.Games;
using Ssit.CrossX.Graphics.Font;
using Ssit.CrossX.Input;
using Ssit.IoC;
using Ssit.CrossX.UI.Services;

namespace RetroGunslinger.Core;

public static class Initializer
{
    public static IIoCContainer InitializeInputMapping(this IIoCContainer container)
    {
        var inputMappings = container.Get<IInputMappings>();
        GameControls.RegisterGameControls(inputMappings);
        
        return container;
    }

    public static void InitializeUiSounds(this IUiApp app)
    {
        var uiSounds = app.Services.Get<IUiSounds>();
        uiSounds
            .AddSound(UiSounds.ItemNavigateSound, "assets:/Sounds/UI/ItemNavigate.wav")
            .AddSound(UiSounds.NavigateToSound, "assets:/Sounds/UI/Navigate.wav")
            .AddSound(UiSounds.NavigateBackSound, "assets:/Sounds/UI/NavigateBack.wav")
            .AddSound(UiSounds.ChangeValueSound, "assets:/Sounds/UI/ChangeValue.wav");
    }

    public static IIoCContainer InitializeMusic(this IIoCContainer container, string startPlaylist)
    {
        var musicPlayer = container.Get<IMusicPlayer>();
            
        musicPlayer.RegisterPlaylist("Menu", 
            new MusicPlaylist()
                .Add("bundle:/Music/Menu.ogg")
        );
        musicPlayer.ChangePlaylist( startPlaylist);
        return container;
    }

    public static IIoCContainer InitializeFonts(this IIoCContainer container)
    {
        var fontsManager = container.Get<IFontsManager>();
        fontsManager.LoadFonts("assets:/Fonts/Fonts.json");
        return container;
    }

    public static void Initialize<TViewModel>(this IUiApp uiApp) where TViewModel: class
    {
        uiApp.InitializeUiSounds();
            
        uiApp.Services.Get<PageInputContext>().AlwaysShowFocus = true;
        uiApp.Services.Get<IPointingDevices>().Enable = false;
        uiApp.Services.Get<ITranslator>().CurrentLanguage = uiApp.Services.Get<ISettingsProvider>().Settings.Language;
        
        uiApp.Navigation.NavigateTo<TViewModel>();
    }

    public static IIoCContainer InitializeGame(this IIoCContainer container)
    {
        container.Get<IContentManager>().RegisterGameContentTypes(GameObject.OriginAlignment.Center | GameObject.OriginAlignment.Bottom);
        return container;
    }

    public static void InitializeNavigation(this INavigationMap map)
    {
        map
            .Map<MainPageViewModel, MainPage>()
            .Map<OptionsPageViewModel, OptionsPage>()
            .Map<OptionsPageInGameViewModel, OptionsPageInGame>()
            .Map<GamePageViewModel, GamePage>()
            .Map<LoadingPageViewModel, LoadingPage>()
            .Map<PausePageViewModel, PausePage>();
    }

    public static void InitializeCustomViews(this IHandlerMapper mapper)
    {
         mapper
             .AddMapping<LabelButtonEx, LabelButtonExHandler>();
    }
}