using System;
using SampleGame.Game.UI.ViewModels;
using Ssit.CrossX;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Values;
using Ssit.CrossX.UI.Views;

namespace SampleGame.Game.UI.Pages;

public class MainPage: Page<MainPageViewModel>
{
    protected override View CreateView()
    {
        
        return new Container
        {
            Children = [
                new Label
                {
                    TextAlign = ContentAlign.Left | ContentAlign.Top,
                    HorizontalAlign = Align.Start,
                    VerticalAlign = Align.Start,
                    Text = "Position: " + ViewModel.Position,
                    TextColor = RgbaColor.White,
                    TextOutlineColor = RgbaColor.Black,
                    Font = ("Default", 16),
                    Padding = (10, 30)
                },
                new Label
                {
                    TextAlign = ContentAlign.Center | ContentAlign.VCenter,
                    AnchorY = 10,
                    VerticalAlign = Align.Start,
                    Text = "Current Time: " + ViewModel.Counter,
                    TextColor = RgbaColor.White,
                    TextOutlineColor = RgbaColor.Black,
                    Font = ("Default", 32),
                    BackgroundColor = RgbaColor.DarkRed,
                    Padding = (10, 10)
                },
                new TextView
                {
                    BackgroundColor = RgbaColor.DarkRed,
                    Text = ViewModel.LongText,
                    TextAlign = ContentAlign.Center | ContentAlign.Top,
                    HorizontalAlign = Align.Center,
                    VerticalAlign = Align.Start,
                    TextColor = RgbaColor.White,
                    TextOutlineColor = RgbaColor.Black,  
                    AnchorY = 100,
                    Width = "50%",
                    Font = ("Default", 24),
                    ParagraphSpacing = "50%",
                    Padding = (10, 10)
                },
                new ImageView
                {
                    BackgroundColor = RgbaColor.Coral,
                    Scaling = ImageScalingMode.AspectFill,
                    Width = 960,
                    Height = 960,
                    HorizontalAlign = Align.Center,
                    VerticalAlign = Align.Center,
                    Source = new Uri("https://picsum.photos/id/368/1280/720", UriKind.Absolute)
                }
            ]
        };
    }
}
