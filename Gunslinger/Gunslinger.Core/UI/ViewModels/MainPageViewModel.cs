using Gunslinger.Core.Game;
using Ssit.CrossX.Commands;
using Ssit.CrossX.Content;
using Ssit.CrossX.Core;
using Ssit.CrossX.Games;
using Ssit.CrossX.Games.Audio;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.IoC;
using Ssit.CrossX.UI.Services;

namespace Gunslinger.Core.UI.ViewModels;

internal class MainPageViewModel
{
    public MainPageViewModel(INavigation navigation, IUiSounds sounds, IAppWindowManager windowManager, IIoCContainer container)
    {
        StartGameCommand = new SyncCommand(() =>
        {
            sounds[UiSounds.NavigateToSound]?.PlayOnce();

            GameInstance gameInstance = null;
            navigation.NavigateTo<LoadingPageViewModel>(new LoadingPageViewModel.Parameters
            {
                OnLoading = () =>
                {
                    gameInstance = container.IoCConstruct<GameInstance>(new GameInstance.Parameters
                    {
                        MapPath = "assets:/Game/Maps/Map1.map",
                        ProcessWorldFunc = GamePhysics.InitPhysicsForWorld
                    });
                    gameInstance.Container.Get<ICommonSoundContainer>().InitGameSounds();
                },
                OnLoaded = () => navigation.NavigateTo<GamePageViewModel>(gameInstance)
            });
        });
        
        OptionsCommand = new SyncCommand(o =>
        {
            sounds[UiSounds.NavigateToSound]?.PlayOnce();
            navigation.NavigateTo<OptionsPageViewModel>();
        });
        
        ExitCommand = new SyncCommand( () =>
        {
            sounds[UiSounds.NavigateToSound]?.PlayOnce();
            windowManager.Close();
        });
    }

    public SyncCommand StartGameCommand { get; }
    
    public SyncCommand OptionsCommand { get; }
    
    public SyncCommand ExitCommand { get; }
}