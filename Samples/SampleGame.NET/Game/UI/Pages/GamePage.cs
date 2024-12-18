using SampleGame.Game.UI.ViewModels;
using Ssit.CrossX;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Views;

namespace SampleGame.Game.UI.Pages;

public class GamePage: Page<GamePageViewModel>
{
    protected override View CreateView()
    {
        return new Container
        {
            Children = [
               new Background
               {
                   BackgroundColor = RgbaColor.Blue
               }
            ]
        };
    }
}