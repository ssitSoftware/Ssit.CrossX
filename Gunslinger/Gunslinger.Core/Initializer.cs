using Gunslinger.Core.UI.Handlers;
using Gunslinger.Core.UI.Pages;
using Gunslinger.Core.UI.ViewModels;
using Gunslinger.Core.UI.Views;
using Ssit.CrossX.Audio;
using Ssit.CrossX.Common;
using Ssit.CrossX.Common.Pages;
using Ssit.CrossX.Content;
using Ssit.CrossX.Games;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Input;
using Ssit.CrossX.IoC;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Services;

namespace Gunslinger.Core;

public static class Initializer
{
    public static IIoCContainer InitializeInputMapping(this IIoCContainer container)
    {
        var inputMappings = container.Get<IInputMappings>();
        var mapper = inputMappings.Mapper(0);

        mapper.MapAxis("Horizontal", GameControllerAxis.LeftX);
        mapper.MapAxis("Horizontal", GameControllerButton.DPadLeft, GameControllerButton.DPadRight);
        mapper.MapAxis("Horizontal", Key.Left, Key.Right);

        mapper.MapButton("Shoot", GameControllerButton.B);
        mapper.MapButton("Shoot", Key.Z);

        mapper.MapButton("Melee", GameControllerButton.X);
        mapper.MapButton("Melee", Key.X);

        mapper.MapButton("Jump", GameControllerButton.A);
        mapper.MapButton("Jump", Key.C);

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

    public static IUiApp InitializeUi(this IIoCContainer container, IInputCoordinateSystem inputCoordinateSystem)
    {
        var uiApp = container.InitializeUi((b, m, h) => OnInitializeUi(b, m, h, inputCoordinateSystem));
        uiApp.InitializeUiSounds();
            
        uiApp.Services.Get<PageInputContext>().AlwaysShowFocus = true;
        uiApp.Services.Get<IPointingDevices>().Enable = false;

        return uiApp;
    }

    public static IIoCContainer InitializeGame(this IIoCContainer container)
    {
        container.Get<IContentManager>().RegisterGameContentTypes();
        return container;
    }
    
    private static void OnInitializeUi(IIoCContainerBuilder builder, INavigationMap navigationMap, IHandlerMapper handlers, IInputCoordinateSystem inputCoordinateSystem)
    {
        builder
            .WithInstance(inputCoordinateSystem)
            .WithCommonUi();
            
        navigationMap
            .Map<MainPageViewModel, MainPage>()
            .Map<OptionsPageViewModel, OptionsPage>()
            .Map<GamePageViewModel, GamePage>();

        handlers
            .AddCommonUiMaping()
            .AddMapping<LabelButtonEx, LabelButtonExHandler>();
    }
}