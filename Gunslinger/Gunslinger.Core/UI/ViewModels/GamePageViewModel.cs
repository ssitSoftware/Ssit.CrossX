using System;
using System.Windows.Input;
using Ssit.CrossX.Commands;
using Ssit.CrossX.Core;
using Ssit.CrossX.IoC;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Services;

namespace Gunslinger.Core.UI.ViewModels;

public class GamePageViewModel: IPageCommandsSource, IDisposable
{
    ICommand IPageCommandsSource.MenuCommand => _backCommand;
    ICommand IPageCommandsSource.BackCommand => null;
    
    private readonly SyncCommand _backCommand;

    public ISimulation Simulation { get; }
    
    public GamePageViewModel(INavigation navigation, IIoCContainer container)
    {
        _backCommand = new SyncCommand(navigation.NavigateBack);
    }

    public void Dispose()
    {
    }
}