using System;
using SampleGame.Services;
using Ssit.CrossX.Commands;
using Ssit.CrossX.UI.Services;

namespace Gunslinger.Core.UI.ViewModels;

internal class MainPageViewModel(INavigation navigation, ITranslator translator)
{
    public SyncCommand StartGameCommand { get; } = new(() => navigation.NavigateTo<GamePageViewModel>());
    public SyncCommand OptionsCommand { get; } = new(o => navigation.NavigateTo<OptionsPageViewModel>());
    public SyncCommand CreditsCommand { get; } = new( ()=>{});
    public SyncCommand LanguageCommand { get; } = new(translator.ToggleLanguage);
}