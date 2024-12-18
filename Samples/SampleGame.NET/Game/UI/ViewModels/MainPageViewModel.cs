using System;
using System.Threading.Tasks;
using Ssit.CrossX.Core;
using Ssit.CrossX.Input;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;

namespace SampleGame.Game.UI.ViewModels;

public class MainPageViewModel
{
    private readonly IEventSource _eventSource;
    private readonly IPointingDevices _pointingDevices;
    public SharedStringValue Counter { get; } = new("");

    public SharedStringValue Position { get; } = new("");

    public SharedString LongText { get; } =
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc a congue nulla, nec gravida eros. Phasellus tempor quam id felis pellentesque ultricies. Curabitur non imperdiet eros. Aliquam rhoncus nibh at nisi euismod, in fermentum sapien convallis. Etiam condimentum lectus vitae nisi ultrices, ut maximus enim tempor. Sed dignissim quis mauris eu interdum. Pellentesque ut malesuada odio. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Mauris ultrices dui vel metus sagittis congue.\n" +
        "Pellentesque vitae accumsan risus. Maecenas consequat nisl vitae finibus auctor. Aliquam non elementum erat. Duis mollis quis dui eget iaculis. In sodales ligula et nisi molestie vestibulum. Aliquam mollis, erat sit amet tincidunt eleifend, tellus metus hendrerit lorem, a aliquet leo risus sit amet turpis. Proin egestas mauris id velit congue, non posuere nisi finibus. Vestibulum eget ligula ligula. Nam ullamcorper felis eget nibh ultrices bibendum.\n" +
        "Maecenas consectetur risus vitae arcu bibendum congue. Cras quis nunc viverra, ultricies nibh quis, sollicitudin nulla. Mauris at purus nec est gravida aliquam eu eu sapien. Donec congue laoreet arcu ut porttitor. Pellentesque et elit id sem luctus posuere tristique in magna. Donec vel cursus felis. Nulla facilisi. Aliquam erat volutpat. Morbi id dui fermentum, dignissim metus eget, elementum neque. Donec in orci sed tellus dignissim laoreet vitae eget nisi.\n" +
        "Ut pellentesque ante eros, vel convallis diam varius eu. Morbi ut eros ac dolor commodo tempor. Vivamus at sagittis odio. Morbi eleifend, tortor non lacinia tincidunt, felis urna pretium dolor, id tincidunt purus quam vel velit. Ut convallis arcu dictum maximus cursus. Duis vel massa eu urna pharetra fermentum at id mauris. Sed id ultrices urna. Morbi diam risus, pulvinar vitae tortor ac, vestibulum dictum quam. Mauris arcu quam, dictum in tincidunt ac, malesuada in nisl. Pellentesque eleifend quam in mollis ultricies.\n" +
        "Nullam finibus commodo tincidunt. Vivamus iaculis congue laoreet. Integer rutrum sollicitudin dolor, consequat dapibus eros fringilla rutrum. Nam ipsum eros, sollicitudin ut turpis eget, aliquet fringilla felis. Praesent ut tempus lorem. Nunc viverra pretium leo, quis bibendum orci congue vel. Sed pretium bibendum velit, eu dignissim libero blandit a. Aenean ullamcorper et nibh nec dignissim. Praesent condimentum nibh sed faucibus iaculis. Nulla nec mauris erat. Fusce egestas urna ut augue ornare, non aliquet magna faucibus. Aliquam mauris erat, tristique ac enim non, consectetur feugiat ligula. In id tortor ac lorem cursus tincidunt.";

    //public ImageSource Image { get; } = new Uri("https://picsum.photos/480/320", UriKind.Absolute);

    public MainPageViewModel(IActionDispatcher actionDispatcher, IEventSource eventSource, IPointingDevices pointingDevices )
    {
        _eventSource = eventSource;
        _pointingDevices = pointingDevices;
        _eventSource.Updating += OnUpdating;
        UpdateTime();
        Task.Run(async () =>
        {
            while (true)
            {
                actionDispatcher.Enqueue(UpdateTime);

                // if (!Image.IsLoading)
                // {
                //     Image.SetSource(new Uri("https://picsum.photos/480/320", UriKind.Absolute));
                // }

                await Task.Delay(100);
            }
        });
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

    private void UpdateTime()
    {
        var now = DateTime.Now;
        Counter.FormatText("{0:00}:{1:00}:{2:00}", now.Hour, now.Minute, now.Second);
    }
}