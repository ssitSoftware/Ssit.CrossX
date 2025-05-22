using RetroGunslinger.Core.Game;
using RetroGunslinger.Core.UI.Styles;
using RetroGunslinger.Core.UI.Views;
using Ssit.CrossX;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Views;
using Ssit.CrossX.Utils;

namespace RetroGunslinger.Core.UI;

public static class DialogPageHelper
{
    public static View CreateDialogLayer(IGameDialogsUi dialogs, bool active)
    {
        return new Container
        {
            Visible = dialogs.Visible,
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
                    Text = dialogs.CurrentText,
                    Font = ("Default", 12),
                    Scaling = TextScaling.Pixel
                },
                new LabelButtonEx
                {
                    UniqueId = "Reply0",
                    HorizontalNavigation = ("Reply2", "Reply1"),
                    Text = dialogs.ReplyOptions[0],
                    Visible = dialogs.ReplyOptionVisible[0],
                    AnchorY = "100%-12",
                    AnchorX = "12",
                    Command = dialogs.ReplyCommand,
                    CommandParameter = 0
                }.WithDialogStyle(Align.Start, active),
                new LabelButtonEx
                {
                    UniqueId = "Reply1",
                    HorizontalNavigation = ("Reply0", "Reply2"),
                    Text = dialogs.ReplyOptions[1],
                    Visible = dialogs.ReplyOptionVisible[1],
                    AnchorY = "100%-12",
                    AnchorX = "50%",
                    Command = dialogs.ReplyCommand,
                    CommandParameter = 1
                }.WithDialogStyle(Align.Center, active),
                new LabelButtonEx
                {
                    UniqueId = "Reply2",
                    HorizontalNavigation = ("Reply1", "Reply0"),
                    Text = dialogs.ReplyOptions[2],
                    Visible = dialogs.ReplyOptionVisible[2],
                    AnchorY = "100%-12",
                    AnchorX = "100%-12",
                    Command = dialogs.ReplyCommand,
                    CommandParameter = 2
                }.WithDialogStyle(Align.End, active)
            ]
        };
    }
}