using System.Windows.Input;
using Ssit.CrossX.Commands;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Services;

namespace SampleGame.Game.UI.ViewModels;

public class GamePageViewModel : IBackCommandSource
{
    ICommand IBackCommandSource.BackCommand => _backCommand;
    private readonly SyncCommand _backCommand;

    public GamePageViewModel(INavigation navigation)
    {
        _backCommand = new SyncCommand(navigation.NavigateBack);
    }
}