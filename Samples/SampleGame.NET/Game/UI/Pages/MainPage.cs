using SampleGame.Game.UI.Styles;
using SampleGame.Game.UI.ViewModels;
using Ssit.CrossX;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;
using Ssit.CrossX.UI.Views;

namespace SampleGame.Game.UI.Pages;

public class MainPage: PageBase<MainPageViewModel>
{
    protected override bool OnUiButton(UiButton button, IInputContext inputContext)
    {
        if (FocusedElement is null)
        {
            var focusable = inputContext.FindFocusable("Start", this);
            inputContext.Focus(focusable, this);
            return true;
        }
        return base.OnUiButton(button, inputContext);
    }

    protected override View CreateView()
    {
        return new Container
        {
            Width = 480,
            HorizontalAlign = Align.Center,
            Padding = (10,10),
            Children = [
                
                // new ScrollView
                // {
                //     BackgroundColor = new (0xff202020),
                //     Width="50%",
                //     Height = "40%",
                //     HorizontalAlign = Align.Start,
                //     VerticalAlign = Align.Center,
                //     ScrollMode  = ScrollMode.Vertical,
                //     UniqueId = "ScrollView",
                //     HorizontalNavigation = ("Button1", "Button1"),
                //     ContentView = new TextView
                //     {
                //         BackgroundColor = new (0xff404040),
                //         Text = ViewModel.LongDesc,
                //         TextAlign = ContentAlign.Justified | ContentAlign.Top,
                //         HorizontalAlign = Align.Fill,
                //         VerticalAlign = Align.Start,
                //         TextColor = RgbaColor.White,
                //         Padding = (10, 10)
                //     }
                // },
                new VerticalStack
                {
                    Padding = (8, 8),
                    Spacing = 8,
                    BackgroundColor = RgbaColor.Brown,
                    VerticalAlign = Align.Center,
                    HorizontalAlign = Align.End,
                    Width = "256",
                    AnchorX = "100%",
                    Children = [
                        new LabelButton
                        {
                            Text = "Start Game",
                            UniqueId = "Start",
                            VerticalNavigation = (null, "Options"),
                            HorizontalNavigation = ("ScrollView", "ScrollView"),
                            Command = ViewModel.Button1Command
                        }.WithDefaultStyle(),

                        new LabelButton
                        {
                            Text = "Options",
                            UniqueId = "Options",
                            VerticalNavigation = ("Start", "Credits"),
                            HorizontalNavigation = ("ScrollView", "ScrollView"),
                            Command = ViewModel.Button2Command
                        }.WithDefaultStyle(),

                        new LabelButton
                        {
                            Text = "Credits",
                            UniqueId = "Credits",
                            VerticalNavigation = ("Options", null),
                            HorizontalNavigation = ("ScrollView", "ScrollView"),
                            Command = ViewModel.Button3Command
                        }.WithDefaultStyle()
                    ]
                }
            ]
        };
    }
}
