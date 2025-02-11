using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Ssit.CrossX.Core;
using Ssit.CrossX.UI;

namespace Gunslinger.Core.UI.ViewModels;

[NoHistory]
internal class LoadingPageViewModel: IDisposable
{
    public class Parameters
    {
        public Action OnLoaded { get; set; }
        public Action OnLoading { get; set; }
    }
    
    private readonly IEventSource _eventSource;
    private readonly Parameters _parameters;
    private Task _task;
    
    public LoadingPageViewModel(IEventSource eventSource, Parameters parameters)
    {
        _eventSource = eventSource;
        _parameters = parameters;
        eventSource.RenderFinished += EventSourceOnRenderFinished;
    }

    private void EventSourceOnRenderFinished()
    { 
        _eventSource.RenderFinished -= EventSourceOnRenderFinished;
        if (_task != null)
        {
            return;
        }

        var stopwatch = new Stopwatch();
        stopwatch.Start();
        _task = Task.Run(_parameters.OnLoading);
        _task.ContinueWith(o =>
        {
            if (stopwatch.ElapsedMilliseconds < 500)
            {
                Task.Delay(500 - (int)stopwatch.ElapsedMilliseconds).Wait();
            }
            _parameters.OnLoaded();
        });
    }

    public void Dispose()
    {
        _eventSource.RenderFinished -= EventSourceOnRenderFinished;
    }
}