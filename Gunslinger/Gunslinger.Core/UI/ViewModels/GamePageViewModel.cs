using System;
using System.Windows.Input;
using Gunslinger.Core.Game;
using Ssit.CrossX.Commands;
using Ssit.CrossX.Core;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.IoC;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;

namespace Gunslinger.Core.UI.ViewModels;

public class GamePageViewModel: IPageCommandsSource, IDisposable
{
    private readonly IEventSource _eventSource;
    private readonly IRenderer2 _renderer;
    ICommand IPageCommandsSource.MenuCommand => _pauseCommand;
    ICommand IPageCommandsSource.BackCommand => null;
    
    private readonly SyncCommand _pauseCommand;
    
    public SharedStringValue Fps { get; } = new();

    public ISimulation Simulation { get; }

    private double _fps = 60;

    public GamePageViewModel(INavigation navigation, IIoCContainer container, IEventSource eventSource, IRenderer2 renderer, ISimulation simulation)
    {
        _eventSource = eventSource;
        _renderer = renderer;
        _pauseCommand = new SyncCommand(()=>navigation.NavigateTo<PausePageViewModel>(Simulation));

        Simulation = simulation;
        
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
        Fps.FormatText("FPS: {0}\n" +
                       "Quads Rendered: {1}\n" +
                       "Sprites Rendered: {2}\n" +
                       "Lines Rendered: {3}\n" +
                       "Rectangles Filled: {4}", 
            (int)Math.Round(_fps),
            _renderer.QuadsRenderer.QuadsRendered,
            _renderer.SpriteRenderer.SpritesRendered,
            _renderer.GeometryRenderer.LinesRendered,
            _renderer.GeometryRenderer.RectanglesFilled);
    }

    public void Dispose()
    {
        Simulation.Dispose();
        _eventSource.Updating -= OnUpdating;
        _eventSource.RenderFinished -= OnRenderFinished;
    }
}