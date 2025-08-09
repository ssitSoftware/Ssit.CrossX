using System.Diagnostics.CodeAnalysis;
using Nokemono.Core.Configuration;
using Nokemono.Core.Game;
using Ssit.CrossX.Commands;
using Ssit.CrossX.Core;
using Ssit.CrossX.Games.Audio;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Games.Logic.Narration;
using Ssit.CrossX.Games.Rendering;
using Ssit.CrossX.UI.Services;
using Ssit.IoC;

namespace Nokemono.Core.UI.ViewModels;

[SuppressMessage("ReSharper", "HeapView.DelegateAllocation")]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Possible")]
[SuppressMessage("ReSharper", "HeapView.ClosureAllocation")]
internal class MainPageViewModel
{
    public SyncCommand StartGameCommand => _startGameCommand ??= new SyncCommand(OnStartGame);
    private SyncCommand _startGameCommand;
    private readonly INavigation _navigation;
    private readonly IUiSounds _sounds;
    private readonly IIoCContainer _container;
    private readonly Config _config;

    public MainPageViewModel(INavigation navigation, IUiSounds sounds, IAppWindowManager windowManager, 
        IIoCContainer container, Config config)
    {
        _navigation = navigation;
        _sounds = sounds;
        _container = container;
        _config = config;
        
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

    public SyncCommand OptionsCommand { get; }
    
    public SyncCommand ExitCommand { get; }

    private void OnStartGame()
    {
        _sounds[UiSounds.NavigateToSound]?.PlayOnce();

        GameInstance gameInstance = null;
        var gameDialogs = _container.IoCConstruct<GameDialogs>();
        
        var particleSystem = _container.IoCConstruct<ParticleSystem>();
        particleSystem.InitGameParticles();
        
        _navigation.NavigateTo<LoadingPageViewModel>(new LoadingPageViewModel.Parameters
        {
            OnLoading = () =>
            {
                gameInstance = _container.IoCConstruct<GameInstance>(new GameInstance.Parameters
                {
                    MapPath = "assets:/Game/Maps/Map01.map",
                    ProcessWorldFunc = GamePhysics.InitPhysicsForWorld,
                    RegisterServices = builder =>
                    {
                        builder
                            .WithInstance<IGameDialogs>(gameDialogs)
                            .WithInstance<IGameDialogsUi>(gameDialogs)
                            .WithInstance<IParticleSystem>(particleSystem)
                            .WithSingleton<IGameState, GameState>()
                            .WithSingleton<INarrationSystem, NarrationSystem>("assets:/Game/Scenario");
                    }
                });

                particleSystem.Attach(gameInstance.World);
                
                gameInstance.Container.Get<ICommonSoundContainer>().InitGameSounds();
                gameInstance.Container.Get<INarrationSystem>().SetValue("playerName", _config.PlayerName);
            },
            OnLoaded = () => _navigation.NavigateTo<GamePageViewModel>(gameInstance.Container.IoCConstruct<GameInterfaces>())
        });
    }
}