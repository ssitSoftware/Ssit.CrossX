using System;
using System.Threading.Tasks;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;

namespace SampleGame.Game.UI.ViewModels;

public class MainPageViewModel
{
    public SharedStringValue Counter { get; } = new ("");
    
    public MainPageViewModel(IActionDispatcher actionDispatcher)
    {
        UpdateTime();
        Task.Run(async () =>
        {
            while (true)
            {
                actionDispatcher.Enqueue(UpdateTime);
                await Task.Delay(100);
            }
        });
    }

    private void UpdateTime()
    {
        var now = DateTime.Now;
        Counter.FormatText("{0:00}:{1:00}:{2:00}", now.Hour, now.Minute, now.Second);
    }
}