using Nokemono.Core.Game;
using Nokemono.Core.UI.Styles;
using Nokemono.Core.UI.Views;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Views;

namespace Nokemono.Core.UI;

public static class DialogPageHelper
{
    public static View CreateDialogLayer(IGameDialogsUi dialogs, bool active, IUiSounds sounds = null)
    {
        return new Container
        {
            Visible = dialogs.Visible,
            BackgroundColor = 2,
            HorizontalAlign = Align.Center,
            VerticalAlign = Align.Start,
            AnchorY = 16,
            Width = "75%",
            Height = "48%",
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
                new VerticalStack
                {
                    Visible = dialogs.Visible,
                    BackgroundColor = 1,
                    VerticalAlign = Align.Center,
                    HorizontalAlign = Align.Center,
                    Width = "90%",
                    Children =
                    [
                        new Label
                        {
                            //BackgroundColor = 2,
                            Width = "90%",
                            HorizontalAlign = Align.Center,
                            VerticalAlign = Align.Center,
                            TextAlign = ContentAlign.Center,
                            TextColor = 2,
                            Text = dialogs.CurrentText,
                            Font = ("Default", 12),
                            Scaling = TextScaling.Pixel,
                            Padding = (0,5, 0, 10)
                        },
                        new Background
                        {
                            Height = 10
                        },
                        new LabelButtonEx
                        {
                            UniqueId = "Reply0",
                            VerticalNavigation = ("Reply2", "Reply1"),
                            Text = dialogs.ReplyOptions[0],
                            Visible = dialogs.ReplyOptionVisible[0],
                            Command = dialogs.ReplyCommand,
                            CommandParameter = 0,
                            CustomSounds = sounds
                        }.WithDialogStyle(),
                        new LabelButtonEx
                        {
                            UniqueId = "Reply1",
                            VerticalNavigation = ("Reply0", "Reply2"),
                            Text = dialogs.ReplyOptions[1],
                            Visible = dialogs.ReplyOptionVisible[1],
                            Command = dialogs.ReplyCommand,
                            CommandParameter = 1,
                            CustomSounds = sounds
                        }.WithDialogStyle(),
                        new LabelButtonEx
                        {
                            UniqueId = "Reply2",
                            VerticalNavigation = ("Reply1", "Reply0"),
                            Text = dialogs.ReplyOptions[2],
                            Visible = dialogs.ReplyOptionVisible[2],
                            Command = dialogs.ReplyCommand,
                            CommandParameter = 2,
                            CustomSounds = sounds
                        }.WithDialogStyle(),
                    ]
                }
            ]
        };
    }
}