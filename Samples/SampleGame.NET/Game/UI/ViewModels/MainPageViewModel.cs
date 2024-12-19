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

    public string LongDesc = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque eget tellus orci. Proin vulputate ligula et urna elementum, vel lacinia purus malesuada. Integer porttitor mi vel purus accumsan malesuada ac vitae enim. Nulla facilisi. Duis dui dui, vestibulum in luctus eget, ornare consequat massa. Proin condimentum leo eget fringilla volutpat. Quisque tristique ultrices elementum. Suspendisse auctor tempus tincidunt. Nunc fringilla sapien at augue facilisis suscipit ut ut tortor. Phasellus pulvinar odio at lacus malesuada dictum. Aliquam in auctor tellus, a hendrerit dui. Donec leo lorem, vehicula a risus eu, pellentesque sagittis ante.\n" +
                                   "Integer vestibulum convallis est, a semper enim feugiat a. Maecenas in condimentum elit, ut imperdiet mauris. Vestibulum purus purus, venenatis vel efficitur non, rutrum in metus. Fusce at semper est, ut gravida velit. Proin tortor diam, efficitur sed ante fringilla, fringilla eleifend orci. Nullam eu maximus ligula. Cras a erat et enim vestibulum bibendum. Vivamus ut odio sem. Donec eleifend tempus quam. Etiam sit amet pulvinar est. Curabitur non neque id dolor euismod volutpat. Morbi tincidunt metus nec scelerisque congue. Proin non gravida leo, at vulputate arcu.\n" +
                                   "Duis ut ipsum sit amet purus dictum pretium vel vel sem. Nullam vel volutpat tellus, a gravida orci. Morbi in accumsan tellus, et congue risus. Donec quis lobortis augue. Mauris efficitur dolor lectus, ut gravida elit vulputate cursus. Duis semper ultrices ante ac tempor. Nam quam purus, efficitur vel lacus lacinia, commodo vestibulum tellus. Nunc tempor ut lectus porta commodo. Sed tempus fermentum leo. Vestibulum lorem ex, elementum non feugiat et, commodo vulputate neque. Donec venenatis, tortor id vehicula commodo, libero ligula pellentesque odio, et auctor libero nisl auctor eros. Nullam et facilisis risus. Ut sit amet erat non quam commodo mollis et sit amet lacus. Suspendisse fermentum at mi et semper. Proin vel arcu finibus, vulputate odio dignissim, aliquet lorem. Phasellus id pretium lorem.\n" +
                                   "Vivamus massa arcu, elementum vel tristique blandit, accumsan at sem. Nunc ut tempor magna, vitae finibus est. Aenean molestie, libero sed finibus efficitur, nibh orci pulvinar tellus, eu feugiat purus libero id ligula. Suspendisse potenti. Donec fringilla hendrerit nunc, et euismod lorem dictum at. Donec nec lacus non lorem pretium ornare. Maecenas rutrum velit quis purus consectetur fermentum.\n" +
                                   "Nunc semper facilisis risus porttitor suscipit. Aenean sagittis quam libero, nec dictum ante ultricies sit amet. Nunc pharetra quis purus a malesuada. Phasellus et iaculis sem. Integer tincidunt faucibus suscipit. Etiam velit erat, pretium vitae eros a, ornare commodo massa. Aliquam eu elit accumsan, euismod ex vel, pellentesque odio. Aliquam erat volutpat. Mauris sed lectus et orci rhoncus dapibus. Nullam tincidunt lacus est, eget consequat urna bibendum quis. Nullam dignissim ut dui non vehicula.";
    
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