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
    private IInputContext _inputContext;
    protected override void OnLoad(IInputContext inputContext)
    {
        base.OnLoad(inputContext);

        _inputContext = inputContext;
        ViewModel.GameInterfaces.Dialogs.FocusElement += DialogsOnFocusElement;
    }

    private void DialogsOnFocusElement(int index)
    {
        if (index < 0)
        {
            _inputContext.Focus(null, this);
            return;
        }
        var focusable = _inputContext.FindFocusable($"Reply{index}", this);
        _inputContext.Focus(focusable, this);
    }

    protected override void OnDispose(bool disposing)
    {
        base.OnDispose(disposing);
        ViewModel.GameInterfaces.Dialogs.FocusElement -= DialogsOnFocusElement;
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