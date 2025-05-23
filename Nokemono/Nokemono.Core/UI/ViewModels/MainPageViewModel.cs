using System.Diagnostics.CodeAnalysis;
using Nokemono.Core.Game;
using Ssit.CrossX.Commands;
using Ssit.CrossX.Core;
using Ssit.CrossX.Games.Audio;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.UI.Services;
using Ssit.IoC;

namespace Nokemono.Core.UI.ViewModels;

[SuppressMessage("ReSharper", "HeapView.DelegateAllocation")]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Possible")]
[SuppressMessage("ReSharper", "HeapView.ClosureAllocation")]
internal class MainPageViewModel
{
    public MainPageViewModel(INavigation navigation, IUiSounds sounds, IAppWindowManager windowManager, IIoCContainer container)
    {
        StartGameCommand = new SyncCommand(() =>
        {
            sounds[UiSounds.NavigateToSound]?.PlayOnce();

            GameInstance gameInstance = null;
            var gameDialogs = container.IoCConstruct<GameDialogs>();
            
            navigation.NavigateTo<LoadingPageViewModel>(new LoadingPageViewModel.Parameters
            {
                OnLoading = () =>
                {
                    gameInstance = container.IoCConstruct<GameInstance>(new GameInstance.Parameters
                    {
                        MapPath = "assets:/Game/Maps/Map01.map",
                        ProcessWorldFunc = GamePhysics.InitPhysicsForWorld,
                        RegisterServices = b =>
                        {
                            b.WithInstance<IGameDialogs>(gameDialogs);
                        }
                    });
                    gameInstance.Container.Get<ICommonSoundContainer>().InitGameSounds();
                },
                OnLoaded = () => navigation.NavigateTo<GamePageViewModel>(new GameInterfaces
                {
                    Instance = gameInstance,
                    Dialogs = gameDialogs
                })
            });
        });
        
        OptionsCommand = new SyncCommand(_ =>
        {
            sounds[UiSounds.NavigateToSound]?.PlayOnce();
            navigation.NavigateTo<OptionsPageViewModel>();
        });
        
        ExitCommand = new SyncCommand( () =>
        {
            sounds[UiSounds.NavigateToSound]?.PlayOnce();
            windowManager.Close();
        });
    }

    public SyncCommand StartGameCommand { get; }
    
    public SyncCommand OptionsCommand { get; }
    
    public SyncCommand ExitCommand { get; }
}