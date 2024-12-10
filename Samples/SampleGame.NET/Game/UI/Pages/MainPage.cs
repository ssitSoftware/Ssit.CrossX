using SampleGame.Game.UI.Templates;
using Ssit.CrossX;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Views;

namespace SampleGame.Game.UI.Pages;

public class MainPage: Page<MainPageViewModel>
{
    protected override View CreateView()
    {
        return new Container
        {
            Padding = (20, 20, 10, 10),
            Children = [
                new Background
                {
                    BackgroundColor = RgbaColor.CornflowerBlue
                }
            ]
        };
    }
}
