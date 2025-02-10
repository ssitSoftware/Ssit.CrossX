using Ssit.CrossX.Commands;
using Ssit.CrossX.Core;
using Ssit.CrossX.UI.Services;

namespace Gunslinger.Core.UI.ViewModels;

internal class MainPageViewModel(INavigation navigation, IUiSounds sounds, IAppWindowManager windowManager)
{
    public SyncCommand StartGameCommand { get; } = new(() =>
    {
        sounds[UiSounds.NavigateToSound]?.PlayOnce();
        navigation.NavigateTo<GamePageViewModel>();
    });
    
    public SyncCommand OptionsCommand { get; } = new(o =>
    {
        sounds[UiSounds.NavigateToSound]?.PlayOnce();
        navigation.NavigateTo<OptionsPageViewModel>();
    });
    
    public SyncCommand ExitCommand { get; } = new( () =>
    {
        sounds[UiSounds.NavigateToSound]?.PlayOnce();
        windowManager.Close();
    });
}