using Gunslinger.Core.UI.ViewModels;
using Ssit.CrossX;
using Ssit.CrossX.Common.Views;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Values;
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
                },
                new Label
                {
                    Text = ViewModel.Fps,
                    AnchorX = 10,
                    AnchorY = 10,
                    VerticalAlign = Align.Start,
                    HorizontalAlign = Align.Start,
                    Font = ("Default", 12),
                    TextColor = RgbaColor.White
                },
                new Label
                {
                    Text = ViewModel.DrawCalls,
                    AnchorX = 10,
                    AnchorY = 30,
                    VerticalAlign = Align.Start,
                    HorizontalAlign = Align.Start,
                    Font = ("Default", 12),
                    TextColor = RgbaColor.White
                }
            ]
        };
    }
}