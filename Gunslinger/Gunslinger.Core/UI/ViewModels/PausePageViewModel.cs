using System.Windows.Input;
using Ssit.CrossX.Commands;
using Ssit.CrossX.Core;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Services;

namespace Gunslinger.Core.UI.ViewModels;

public class PausePageViewModel(INavigation navigation, ISimulation simulation) : IPageCommandsSource
{
    public ISimulation Simulation { get; } = simulation;
    public ICommand ResumeCommand { get; } = new SyncCommand(navigation.NavigateBack);
    public ICommand OptionsCommand { get; } = new SyncCommand(()=>navigation.NavigateTo<OptionsPageInGameViewModel>(simulation));
    public ICommand ExitCommand { get; } = new SyncCommand(navigation.NavigateBackTo<MainPageViewModel>);
    public ICommand BackCommand => ResumeCommand;
    public ICommand MenuCommand => ResumeCommand;
}