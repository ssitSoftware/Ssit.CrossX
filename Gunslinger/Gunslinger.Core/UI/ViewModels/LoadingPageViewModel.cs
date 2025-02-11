using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Ssit.CrossX.Core;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Values;

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

    private float _ticks;

    private const string Characters = "!@#$%^&*()_+=-\\<>/[]{}~";

    private char[] _chars = new char[10];
    
    public SharedStringValue DotsText { get; } = new();
    
    private readonly Random _random = new((int)DateTime.Now.Ticks);
    
    public LoadingPageViewModel(IEventSource eventSource, Parameters parameters)
    {
        _eventSource = eventSource;
        _parameters = parameters;
        
        eventSource.Updating += EventSourceOnUpdating;
        eventSource.RenderFinished += EventSourceOnRenderFinished;
        
        for (var idx = 0; idx < 10; ++idx)
        {
            _chars[idx] = Characters[_random.Next(Characters.Length)];
        }
    }

    private void EventSourceOnUpdating(float dt)
    {
        _ticks += dt * 20;
        while (_ticks >= 1)
        {
            _ticks -= 1;
            _chars[_random.Next(_chars.Length)] = Characters[_random.Next(Characters.Length)];
            UpdateText();
        }
    }

    private void UpdateText()
    {
        DotsText.FormatText("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}", _chars[0], _chars[1], _chars[2], _chars[3], _chars[4], _chars[5], _chars[6], _chars[7], _chars[8], _chars[9]);
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
            _eventSource.Updating -= EventSourceOnUpdating;
        });
    }

    public void Dispose()
    {
        _eventSource.Updating -= EventSourceOnUpdating;
        _eventSource.RenderFinished -= EventSourceOnRenderFinished;
    }
}