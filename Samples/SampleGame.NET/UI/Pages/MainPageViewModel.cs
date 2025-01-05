using SampleGame.Services;
using Ssit.CrossX.Commands;
using Ssit.CrossX.UI.Services;

namespace SampleGame.UI.Pages;

public class MainPageViewModel(INavigation navigation, ITranslator translator)
{
    public SyncCommand StartGameCommand { get; } = new(o => { });
    public SyncCommand OptionsCommand { get; } = new(o => navigation.NavigateTo<OptionsPageViewModel>());
    public SyncCommand CreditsCommand { get; } = new(o => navigation.NavigateTo<GamePageViewModel>());
    public SyncCommand ExitCommand { get;  } = new(o => { });
    public SyncCommand LanguageCommand { get; } = new(translator.ToggleLanguage);
}