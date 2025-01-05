using System.Windows.Input;
using Ssit.CrossX.Commands;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;

namespace SampleGame.UI.Pages;

public class GamePageViewModel(INavigation navigation) : IPageCommandsSource
{
    ICommand IPageCommandsSource.MenuCommand => _backCommand;
    ICommand IPageCommandsSource.BackCommand => null;
    
    private readonly SyncCommand _backCommand = new(navigation.NavigateBack);
    
    public SharedValue<int> MaxHitPoints { get; } = new(8);
    public SharedValue<int> HitPoints { get; } = new(4);
    
    public SharedValue<int> MaxRounds { get; } = new(6);
    public SharedValue<int> Rounds { get; } = new(5);
}