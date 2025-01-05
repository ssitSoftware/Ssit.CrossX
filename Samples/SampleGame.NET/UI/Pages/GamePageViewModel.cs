using System.Windows.Input;
using Ssit.CrossX.Commands;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Services;

namespace SampleGame.UI.Pages;

public class GamePageViewModel(INavigation navigation) : IPageCommandsSource
{
    ICommand IPageCommandsSource.MenuCommand => null;
    ICommand IPageCommandsSource.BackCommand => _backCommand;
    private readonly SyncCommand _backCommand = new(navigation.NavigateBack);
}