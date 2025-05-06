using RetroGunslinger.Core.UI.ViewModels;
using Ssit.CrossX;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Views;

namespace RetroGunslinger.Core.UI.Pages;

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
                    GameInstance = ViewModel.GameInstance,
                    ShowDebug = ViewModel.ShowDebug
                },
                new Label
                {
                    Text = ViewModel.Fps,
                    AnchorX = 4,
                    AnchorY = 4,
                    VerticalAlign = Align.Start,
                    HorizontalAlign = Align.Start,
                    TextAlign = ContentAlign.Left | ContentAlign.Top,
                    Font = ("Default", 12),
                    TextColor = Palette.Foreground,
                    TextOutlineColor = Palette.Background,
                    Scaling = TextScaling.Default,
                    Visible = ViewModel.ShowFps
                }
            ]
        };
    }
}