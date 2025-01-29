using Gunslinger.Core.UI.ViewModels;
using Ssit.CrossX.Common.Views;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Views;

namespace Gunslinger.Core.UI.Pages;

public class GamePage: Page<GamePageViewModel>
{
    protected override View CreateView()
    {
        return new Container
        {
            Children =
            [
                new GameView
                {
                    Simulation = ViewModel.Simulation
                }
            ]
        };
    }
}