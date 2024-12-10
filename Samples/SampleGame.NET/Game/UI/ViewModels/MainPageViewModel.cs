using System.Threading.Tasks;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;

namespace SampleGame.Game.UI.ViewModels;

public class MainPageViewModel
{
    public SharedStringValue Counter { get; } = new ("0");
    private int _counter = 0;

    public MainPageViewModel(IActionDispatcher actionDispatcher)
    {
        Task.Run(async () =>
        {
            for (var idx = 0; idx < 10000; idx++)
            {
                await Task.Delay(100);
                _counter++;

                actionDispatcher.Enqueue(() => Counter.FormatText("{0}", _counter));
            }
        });
    }
}