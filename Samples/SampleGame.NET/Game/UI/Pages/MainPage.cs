using SampleGame.Game.UI.Templates;
using SampleGame.Game.UI.ViewModels;
using Ssit.CrossX;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Views;

namespace SampleGame.Game.UI.Pages;

public class MainPage: Page<MainPageViewModel>
{
    protected override View CreateView()
    {
        return new Container
        {
            Padding = ("10%", "10%"),
            Children = [
                new Background
                {
                    BackgroundColor = RgbaColor.CornflowerBlue,
                    Width = 100,
                    Height = 100,
                    AnchorX= "25%",
                    HorizontalAlign = Align.Center,
                    VerticalAlign = Align.Center
                },
                new Background
                {
                    BackgroundColor = RgbaColor.OrangeRed,
                    Width = 100,
                    Height = 100,
                    AnchorX= "75%",
                    HorizontalAlign = Align.Center,
                    VerticalAlign = Align.Center
                },
                new Label
                {
                    TextAlign = TextAlign.Center | TextAlign.VCenter,
                    Text = ViewModel.Counter,
                    TextColor = RgbaColor.White
                }
            ]
        };
    }
}
