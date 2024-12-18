using System;
using System.Threading.Tasks;
using Ssit.CrossX.Commands;
using Ssit.CrossX.Core;
using Ssit.CrossX.Input;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;

namespace SampleGame.Game.UI.ViewModels;

public class MainPageViewModel
{
    private readonly IEventSource _eventSource;
    private readonly IPointingDevices _pointingDevices;
    
    public SharedStringValue Position { get; } = new ("");

    public SyncCommand Button1Command { get; }
    public SyncCommand Button2Command { get; }
    public SyncCommand Button3Command { get; }
    
    public MainPageViewModel(IActionDispatcher actionDispatcher, IEventSource eventSource, IPointingDevices pointingDevices, INavigation navigation )
    {
        Button1Command = new SyncCommand(o => { });
        Button2Command = new SyncCommand(o => { });
        Button3Command = new SyncCommand(o => navigation.NavigateTo<GamePageViewModel>());
        
        _eventSource = eventSource;
        _pointingDevices = pointingDevices;
        _eventSource.Updating += OnUpdating;
    }

    private void OnUpdating(float _)
    {
        if (_pointingDevices.HoverPosition.HasValue)
        {
            Position.FormatText("{0:0}, {1:0}", _pointingDevices.HoverPosition.Value.X, _pointingDevices.HoverPosition.Value.Y);
        }
        else
        {
            Position.FormatText("Outside");
        }
    }
}