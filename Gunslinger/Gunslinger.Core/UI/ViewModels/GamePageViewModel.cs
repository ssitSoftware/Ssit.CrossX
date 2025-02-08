using System;
using System.Windows.Input;
using Gunslinger.Core.Game;
using Ssit.CrossX.Commands;
using Ssit.CrossX.Core;
using Ssit.CrossX.Graphics;
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
    
    private readonly IRenderer _renderer;

    public GamePageViewModel(INavigation navigation, IIoCContainer container, IEventSource eventSource, IRenderingWindow renderingWindow)
    {
        _renderer = renderingWindow.Renderer;
        
        _eventSource = eventSource;
        _backCommand = new SyncCommand(navigation.NavigateBack);
        
        Simulation = container.IoCConstruct<Simulation>("assets:/Game/Maps/Map1.map");

        _eventSource.Updating += OnUpdating;
        _eventSource.RenderFinished += OnRenderFinished;
    }

    private void OnRenderFinished()
    {
        DrawCalls.FormatText("{0}", _renderer.DrawCalls);
    }

    private void OnUpdating(float dt)
    {
        var fps = (int)MathF.Round(1f / MathF.Max(0.00000001f, dt));
        Fps.FormatText("{0}", fps);
    }

    public void Dispose()
    {
        Simulation.Dispose();
        _eventSource.Updating -= OnUpdating;
        _eventSource.RenderFinished -= OnRenderFinished;
    }
}