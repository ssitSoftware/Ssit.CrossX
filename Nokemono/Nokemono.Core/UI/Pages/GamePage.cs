using Nokemono.Core.UI.ViewModels;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Views;

namespace Nokemono.Core.UI.Pages;

public class GamePage: Page<GamePageViewModel>
{
    private IUiSounds _gameUiSounds;
    
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

    protected override void OnDispose(bool disposing)
    {
        base.OnDispose(disposing);
        _gameUiSounds?.Dispose();
        _gameUiSounds = null;
    }

    protected override View CreateView()
    {
        _gameUiSounds = Services.IoCConstruct<UiSoundsContainer>();
        _gameUiSounds
            .AddSound(UiSounds.ExecuteSound, "assets:/Sounds/UI/DialogSelect.wav")
            .AddSound(UiSounds.ItemNavigateSound, "assets:/Sounds/UI/DialogMove.wav");
        
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
                DialogPageHelper.CreateDialogLayer(ViewModel.GameInterfaces.Dialogs, true, _gameUiSounds),
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
                    Scaling = TextScaling.Pixel,
                    Visible = ViewModel.ShowFps
                }
            ]
        };
    }
}