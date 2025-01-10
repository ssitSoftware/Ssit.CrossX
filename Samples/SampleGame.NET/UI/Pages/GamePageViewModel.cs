using System;
using System.Numerics;
using System.Windows.Input;
using SampleGame.Game.Actors;
using SampleGame.Game.Logic;
using Ssit.CrossX.Commands;
using Ssit.CrossX.IoC;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;

namespace SampleGame.UI.Pages;

public class GamePageViewModel : IPageCommandsSource, IDisposable
{
    ICommand IPageCommandsSource.MenuCommand => _backCommand;
    ICommand IPageCommandsSource.BackCommand => null;
    
    public SharedValue<int> MaxHitPoints { get; } = new(8);
    public SharedValue<int> HitPoints { get; } = new(4);
    
    public SharedValue<int> MaxRounds { get; } = new(6);
    public SharedValue<int> Rounds { get; } = new(5);
    
    public Simulation Simulation { get; }
    
    private readonly SyncCommand _backCommand;
    private readonly IIoCContainer _container;
    
    public GamePageViewModel(INavigation navigation, IIoCContainer container)
    {
        _backCommand = new SyncCommand(navigation.NavigateBack);
        
        Simulation = new Simulation(Vector2.Zero, 12);
        
        _container = IoC.NewBuilder().WithParent(container).WithInstance(Simulation).Build();
        var playerObject = _container.IoCConstruct<PlayerObject>();
        
        Simulation.Camera = playerObject.Camera;
        Simulation.Updated += () =>
        {
            MaxRounds.Value = playerObject.Gun.MaxShots;
            Rounds.Value = playerObject.Gun.ShotsLeft;
        };
    }

    public void Dispose()
    {
        _container?.Dispose();
    }
}