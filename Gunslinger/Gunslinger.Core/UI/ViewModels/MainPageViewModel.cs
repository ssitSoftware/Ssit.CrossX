using Gunslinger.Core.Game;
using Ssit.CrossX.Commands;
using Ssit.CrossX.Core;
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

            ISimulation simulation = null;
            navigation.NavigateTo<LoadingPageViewModel>(new LoadingPageViewModel.Parameters
            {
                OnLoading = () => simulation = container.IoCConstruct<Simulation>("assets:/Game/Maps/Map1.map"),
                OnLoaded = () => navigation.NavigateTo<GamePageViewModel>(simulation)
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