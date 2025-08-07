using System.Diagnostics.CodeAnalysis;
using Nokemono.Core.Configuration;
using Nokemono.Core.Game;
using Ssit.CrossX.Commands;
using Ssit.CrossX.Core;
using Ssit.CrossX.Games.Audio;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Games.Logic.Narration;
using Ssit.CrossX.UI.Common.Services;
using Ssit.CrossX.UI.Services;
using Ssit.IoC;

namespace Nokemono.Core.UI.ViewModels;

[SuppressMessage("ReSharper", "HeapView.DelegateAllocation")]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Possible")]
[SuppressMessage("ReSharper", "HeapView.ClosureAllocation")]
internal class MainPageViewModel(INavigation navigation, IUiSounds sounds, IAppWindowManager windowManager, IIoCContainer container, Config config)
{
    public SyncCommand StartGameCommand => _startGameCommand ??= new SyncCommand(OnStartGame);
    private SyncCommand _startGameCommand;
    
    public SyncCommand OptionsCommand { get; } = new(_ =>
    {
        sounds[UiSounds.NavigateToSound]?.PlayOnce();
        navigation.NavigateTo<OptionsPageViewModel>();
    });
    
    public SyncCommand ExitCommand { get; } = new( () =>
    {
        sounds[UiSounds.NavigateToSound]?.PlayOnce();
        windowManager.Close();
    });

    private void OnStartGame()
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
                    RegisterServices = builder =>
                    {
                        builder
                            .WithInstance<IGameDialogs>(gameDialogs)
                            .WithInstance<IGameDialogsUi>(gameDialogs)
                            .WithSingleton<IGameState, GameState>()
                            .WithSingleton<INarrationSystem, NarrationSystem>("assets:/Game/Scenario");
                    }
                });
                gameInstance.Container.Get<ICommonSoundContainer>().InitGameSounds();
                gameInstance.Container.Get<INarrationSystem>().SetValue("playerName", config.PlayerName);
            },
            OnLoaded = () => navigation.NavigateTo<GamePageViewModel>(gameInstance.Container.IoCConstruct<GameInterfaces>())
        });
    }
}