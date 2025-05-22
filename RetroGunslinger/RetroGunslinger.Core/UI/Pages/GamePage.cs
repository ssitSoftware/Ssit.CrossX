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
                    Active = true
                },
                DialogPageHelper.CreateDialogLayer(ViewModel.GameInterfaces.Dialogs, true),
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