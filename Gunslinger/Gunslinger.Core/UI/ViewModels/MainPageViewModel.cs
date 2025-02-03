using System;
using SampleGame.Services;
using Ssit.CrossX.Commands;
using Ssit.CrossX.UI.Services;

namespace Gunslinger.Core.UI.ViewModels;

internal class MainPageViewModel(INavigation navigation)
{
    public SyncCommand StartGameCommand { get; } = new(() => navigation.NavigateTo<GamePageViewModel>());
    public SyncCommand OptionsCommand { get; } = new(o => navigation.NavigateTo<OptionsPageViewModel>());
    public SyncCommand CreditsCommand { get; } = new( ()=>{});
}