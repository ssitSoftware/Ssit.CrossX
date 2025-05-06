using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using Ssit.CrossX.Commands;
using Ssit.CrossX.Core;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Services;

namespace RetroGunslinger.Core.UI.ViewModels;

[SuppressMessage("ReSharper", "HeapView.DelegateAllocation")]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class PausePageViewModel(INavigation navigation, IGameInstance gameInstance) : IPageCommandsSource
{
    public IGameInstance GameInstance { get; } = gameInstance;
    public ICommand ResumeCommand { get; } = new SyncCommand(navigation.NavigateBack);
    public ICommand OptionsCommand { get; } = new SyncCommand(()=>navigation.NavigateTo<OptionsPageInGameViewModel>(gameInstance));
    public ICommand ExitCommand { get; } = new SyncCommand(navigation.NavigateBackTo<MainPageViewModel>);
    public ICommand BackCommand => ResumeCommand;
    public ICommand MenuCommand => ResumeCommand;
}