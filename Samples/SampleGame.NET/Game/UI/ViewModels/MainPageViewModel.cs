using Ssit.CrossX.Commands;
using Ssit.CrossX.UI.Services;

namespace SampleGame.Game.UI.ViewModels;

public class MainPageViewModel(INavigation navigation)
{
    public SyncCommand StartGameCommand { get; } = new(o => { });
    public SyncCommand OptionsCommand { get; } = new(o => { });
    public SyncCommand CreditsCommand { get; } = new(o => navigation.NavigateTo<GamePageViewModel>());
    public SyncCommand ExitCommand { get;  } = new(o => { });
}