using Ssit.Pixel.UI;
using Ssit.Pixel.UI.Parameters;
using Ssit.Pixel.UI.Views;

namespace SampleGame.Game.UI;

public class MainPage: Page<MainPageViewModel>
{
    protected override View CreateView()
    {
        return new Container
        {
            Children = 
            [
                new Label
                {
                    Text = ViewModel.Title,
                    AnchorX = 10,
                    AnchorY = 10,
                    Width = 100,
                    Height = 100,
                    HorizontalAlign = Align.Center,
                    VerticalAlign = Align.Start
                }
            ]
        };
    }
}