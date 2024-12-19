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
            var focusable = inputContext.FindFocusable("Button1", this);
            inputContext.Focus(focusable, this);
            return true;
        }
        return base.OnUiButton(button, inputContext);
    }

    protected override View CreateView()
    {
        return new Container
        {
            Padding = (10,10),
            Children = [
                
                new ScrollView
                {
                    BackgroundColor = new (0xff202020),
                    Width="50%",
                    Height = "50%",
                    HorizontalAlign = Align.Center,
                    VerticalAlign = Align.Start,
                    ScrollMode  = ScrollMode.Vertical,
                    ContentView = new TextView
                    {
                        BackgroundColor = new (0xff404040),
                        Text = ViewModel.LongDesc,
                        TextAlign = ContentAlign.Justified | ContentAlign.Top,
                        HorizontalAlign = Align.Fill,
                        VerticalAlign = Align.Start,
                        TextColor = RgbaColor.White,
                        Padding = (10, 10)
                    }
                },
                new VerticalStack
                {
                    Padding = (20, 20),
                    Spacing = 20,
                    BackgroundColor = RgbaColor.Brown,
                    VerticalAlign = Align.Center,
                    HorizontalAlign = Align.Center,
                    Children = [
                        new LabelButton
                        {
                            Text = "New Game",
                            UniqueId = "Button1",
                            VerticalNavigation = ("Button3", "Button2"),
                            Command = ViewModel.Button1Command
                        }.WithDefaultStyle(),

                        new LabelButton
                        {
                            Text = "Options",
                            UniqueId = "Button2",
                            VerticalNavigation = ("Button1", "Button3"),
                            Command = ViewModel.Button2Command
                        }.WithDefaultStyle(),

                        new LabelButton
                        {
                            Text = "Credits",
                            UniqueId = "Button3",
                            VerticalNavigation = ("Button2", "Button1"),
                            Command = ViewModel.Button3Command
                        }.WithDefaultStyle()
                    ]
                }
            ]
        };
    }
}
