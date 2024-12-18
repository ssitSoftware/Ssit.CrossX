using SampleGame.Game.UI.ViewModels;
using Ssit.CrossX;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Views;

namespace SampleGame.Game.UI.Pages;

public static class Styles
{
    public static LabelButton ApplyStyle(this LabelButton button)
    {
        button.TextAlign = ContentAlign.Center | ContentAlign.VCenter;
        button.VerticalAlign = Align.Start;
        button.Font = ("Default", 32);
        button.Padding = (4, 4);
        button.TextColor = RgbaColor.DarkGray;
        button.HoverTextColor = RgbaColor.LightGray;
        button.FocusedTextColor = RgbaColor.Yellow;
        button.DisabledTextColor = new(0xff494949);
        return button;
    }
}

public class MainPage: Page<MainPageViewModel>
{
    protected override void OnLoad(IInputContext inputContext)
    {
        var focusable = inputContext.FindFocusable("Button1", this);
        inputContext.Focus(focusable, this);
    }

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
                    Padding = (10, 30),
                },
                new LabelButton
                {
                    AnchorY = 10,
                    Text = "Current Time: " + ViewModel.Counter,
                    UniqueId = "Button1",
                    VerticalNavigation = ("Button3", "Button2"),
                    Command = ViewModel.Button1Command
                }.ApplyStyle(),
                new LabelButton
                {
                    AnchorY = 80,
                    Text = "Button 2",
                    UniqueId = "Button2",
                    VerticalNavigation = ("Button1", "Button3"),
                    Command = ViewModel.Button2Command
                }.ApplyStyle(),
                new LabelButton
                {
                    AnchorY = 150,
                    Text = "Przycisk ładny i piękny nr 3",
                    UniqueId = "Button3",
                    VerticalNavigation = ("Button2", "Button1"),
                    Command = ViewModel.Button3Command
                }.ApplyStyle()
            ]
        };
    }
}
