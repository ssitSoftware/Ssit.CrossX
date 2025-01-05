using SampleGame.Game.UI.ViewModels;
using Ssit.CrossX;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Views;

namespace SampleGame.Game.UI.Pages;

public class GamePage: Page<GamePageViewModel>
{
    protected override View CreateView()
    {
        return new Container
        {
            BackgroundColor = RgbaColor.Black,
            Children = [
               new Background
               {
                   BackgroundColor = RgbaColor.Blue,
                   Width = 100,
                   Height = 100,
                   VerticalAlign = Align.Center,
                   HorizontalAlign = Align.Center
               }
            ]
        };
    }
}