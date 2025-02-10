using System;
using System.Windows.Input;
using Gunslinger.Core.Game;
using Ssit.CrossX.Commands;
using Ssit.CrossX.Core;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.IoC;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;

namespace Gunslinger.Core.UI.ViewModels;

public class GamePageViewModel: IPageCommandsSource, IDisposable
{
    private readonly IEventSource _eventSource;
    ICommand IPageCommandsSource.MenuCommand => _backCommand;
    ICommand IPageCommandsSource.BackCommand => null;
    
    private readonly SyncCommand _backCommand;
    
    public SharedStringValue Fps { get; } = new();

    public ISimulation Simulation { get; }
    public SharedStringValue DrawCalls { get; } = new();

    private double _fps = 60;

    public GamePageViewModel(INavigation navigation, IIoCContainer container, IEventSource eventSource)
    {
        _eventSource = eventSource;
        _backCommand = new SyncCommand(navigation.NavigateBack);
        
        Simulation = container.IoCConstruct<Simulation>("assets:/Game/Maps/Map1.map");

        _eventSource.Updating += OnUpdating;
        _eventSource.RenderFinished += OnRenderFinished;
    }

    private void OnRenderFinished()
    {
    }

    private void OnUpdating(float dt)
    {
        var fps = 1.0 / Math.Max(0.00000001, dt);
        _fps = (fps * 2 + _fps * 8) / 10.0;
        Fps.FormatText("{0}", (int)Math.Round(_fps));
    }

    public void Dispose()
    {
        Simulation.Dispose();
        _eventSource.Updating -= OnUpdating;
        _eventSource.RenderFinished -= OnRenderFinished;
    }
}