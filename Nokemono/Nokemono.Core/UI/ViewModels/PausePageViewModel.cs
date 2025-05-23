using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using Nokemono.Core.Game;
using Ssit.CrossX.Commands;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Services;

namespace Nokemono.Core.UI.ViewModels;

[SuppressMessage("ReSharper", "HeapView.DelegateAllocation")]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class PausePageViewModel : IPageCommandsSource
{
    public PausePageViewModel(INavigation navigation, IGameInterfaces gameInterfaces, IUiSounds sounds)
    {
        GameInterfaces = gameInterfaces;
        
        BackCommand = new SyncCommand( ()=>
        {
            sounds[UiSounds.NavigateBackSound]?.PlayOnce();
            navigation.NavigateBack();
        });
        ResumeCommand = new SyncCommand( ()=>
        {
            sounds[UiSounds.NavigateToSound]?.PlayOnce();
            navigation.NavigateBack();
        });
        OptionsCommand = new SyncCommand(() =>
        {
            sounds[UiSounds.NavigateToSound]?.PlayOnce();
            navigation.NavigateTo<OptionsPageInGameViewModel>(gameInterfaces);
        });
        ExitCommand = new SyncCommand( ()=>
        {
            sounds[UiSounds.NavigateToSound]?.PlayOnce();
            navigation.NavigateBackTo<MainPageViewModel>();
        });
        
        sounds[UiSounds.MenuSound]?.PlayOnce();
    }

    public IGameInterfaces GameInterfaces { get; }
    public ICommand ResumeCommand { get; }
    public ICommand OptionsCommand { get; }
    
    public ICommand ExitCommand { get; }
    
    public ICommand BackCommand { get; }
    public ICommand MenuCommand => BackCommand;
}