using SampleGame.Game.UI.Styles;
using SampleGame.Game.UI.ViewModels;
using Ssit.CrossX;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;
using Ssit.CrossX.UI.Views;

namespace SampleGame.Game.UI.Pages;

public class MainPage: Page<MainPageViewModel>
{
    protected override bool OnUiButton(UiButton button, IInputContext inputContext)
    {
        switch (button)
        {
            case UiButton.Down:
            case UiButton.Up:

                if (FocusedElement is null)
                {
                    var focusable = inputContext.FindFocusable("Button1", this);
                    inputContext.Focus(focusable, this);
                    return true;
                }
                break;
        }
        
        return base.OnUiButton(button, inputContext);
    }

    protected override View CreateView()
    {
        return new Container
        {
            Padding = (20, 20),
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
                    Padding = (10, 30),
                },

                new VerticalStack
                {
                    Padding = (32, 32),
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
