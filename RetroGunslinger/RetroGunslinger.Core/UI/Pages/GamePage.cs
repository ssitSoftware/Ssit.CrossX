using RetroGunslinger.Core.UI.Styles;
using RetroGunslinger.Core.UI.ViewModels;
using RetroGunslinger.Core.UI.Views;
using Ssit.CrossX;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Views;

namespace RetroGunslinger.Core.UI.Pages;

public class GamePage: Page<GamePageViewModel>
{
    protected override void OnLoad(IInputContext inputContext)
    {
        base.OnLoad(inputContext);

        ViewModel.GameInterfaces.Dialogs.FocusElement += i =>
        {
            if (i < 0)
            {
                inputContext.Focus(null, this);
                return;
            }
            var focusable = inputContext.FindFocusable($"Reply{i}", this);
            inputContext.Focus(focusable, this);
        };
    }

    protected override View CreateView()
    {
        return new Container
        {
            Children =
            [
                new GameView
                {
                    GameInstance = ViewModel.GameInterfaces.Instance,
                    ShowDebug = ViewModel.ShowDebug,
                    Active = !ViewModel.GameInterfaces.Dialogs.Visible
                },
                new Container
                {
                    Visible = ViewModel.GameInterfaces.Dialogs.Visible,
                    BackgroundColor = RgbaColor.Yellow,
                    HorizontalAlign = Align.Center,
                    VerticalAlign = Align.Center,
                    AnchorY = "30%",
                    Width = "75%",
                    Height = "30%",
                    Children = 
                    [
                        new Background
                        {
                            BackgroundColor = 2,
                        },
                        new Background
                        {
                            HorizontalAlign = Align.Center,
                            VerticalAlign = Align.Center,
                            Width = "100%-2",
                            Height = "100%-2",
                            BackgroundColor = 1,
                        },
                        new Background
                        {
                            HorizontalAlign = Align.Center,
                            VerticalAlign = Align.Center,
                            Width = "100%-4",
                            Height = "100%-4",
                            BackgroundColor = 2,
                        },
                        new Background
                        {
                            HorizontalAlign = Align.Center,
                            VerticalAlign = Align.Center,
                            Width = "100%-6",
                            Height = "100%-6",
                            BackgroundColor = 1,
                        },
                        new Label
                        {
                            HorizontalAlign = Align.Fill,
                            VerticalAlign = Align.Start,
                            AnchorY = "12",
                            TextColor = 2,
                            Text = ViewModel.GameInterfaces.Dialogs.CurrentText,
                            Font = ("Default", 12),
                            Scaling = TextScaling.Pixel
                        },
                        new LabelButtonEx
                        {
                            UniqueId = "Reply0",
                            HorizontalNavigation = ("Reply2", "Reply1"),
                            Text = ViewModel.GameInterfaces.Dialogs.ReplyOptions[0],
                            Visible = ViewModel.GameInterfaces.Dialogs.ReplyOptionVisible[0],
                            AnchorY = "100%-12",
                            AnchorX = "12",
                            Command = ViewModel.GameInterfaces.Dialogs.ReplyCommand,
                            CommandParameter = 0
                        }.WithDialogStyle(Align.Start),
                        new LabelButtonEx
                        {
                            UniqueId = "Reply1",
                            HorizontalNavigation = ("Reply0", "Reply2"),
                            Text = ViewModel.GameInterfaces.Dialogs.ReplyOptions[1],
                            Visible = ViewModel.GameInterfaces.Dialogs.ReplyOptionVisible[1],
                            AnchorY = "100%-12",
                            AnchorX = "50%",
                            Command = ViewModel.GameInterfaces.Dialogs.ReplyCommand,
                            CommandParameter = 1
                        }.WithDialogStyle(Align.Center),
                        new LabelButtonEx
                        {
                            UniqueId = "Reply2",
                            HorizontalNavigation = ("Reply1", "Reply0"),
                            Text = ViewModel.GameInterfaces.Dialogs.ReplyOptions[2],
                            Visible = ViewModel.GameInterfaces.Dialogs.ReplyOptionVisible[2],
                            AnchorY = "100%-12",
                            AnchorX = "100%-12",
                            Command = ViewModel.GameInterfaces.Dialogs.ReplyCommand,
                            CommandParameter = 2
                        }.WithDialogStyle(Align.End)
                    ]
                },
                new Label
                {
                    Text = ViewModel.Fps,
                    AnchorX = 4,
                    AnchorY = 4,
                    VerticalAlign = Align.Start,
                    HorizontalAlign = Align.Start,
                    TextAlign = ContentAlign.Left | ContentAlign.Top,
                    Font = ("Default", 12),
                    TextColor = Palette.Foreground,
                    TextOutlineColor = Palette.Background,
                    Scaling = TextScaling.Default,
                    Visible = ViewModel.ShowFps
                }
            ]
        };
    }
}