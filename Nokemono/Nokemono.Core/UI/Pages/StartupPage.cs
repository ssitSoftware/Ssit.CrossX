using Nokemono.Core.UI.ViewModels;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Values;
using Ssit.CrossX.UI.Views;

namespace Nokemono.Core.UI.Pages;

public class StartupPage: Page<StartupPageViewModel>
{
    protected override View CreateView()
    {
        return new Container
        {
            BackgroundColor = 1,
            Children = [
                new ImageView
                {
                    Source = "assets:/UI/PoweredBy.png!",
                    AnchorX = "100%-10",
                    AnchorY = "100%-10",
                    HorizontalAlign = Align.End,
                    VerticalAlign = Align.End,
                    Scaling = ImageScalingMode.None,
                    Width = Length.Auto,
                    Height = Length.Auto
                }
            ]
        };
    }
}