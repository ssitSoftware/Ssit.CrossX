using System.Threading.Tasks;
using Ssit.CrossX.UI.Values;

namespace SampleGame.Game.UI.ViewModels;

public class MainPageViewModel
{
    public SharedStringValue Counter { get; } = new SharedStringValue("0");
    private int _counter = 0;

    public MainPageViewModel()
    {
        Task.Run(async () =>
        {
            await Task.Delay(100);
            _counter++;
            Counter.FormatText("{0}", _counter);
        });
    }
}