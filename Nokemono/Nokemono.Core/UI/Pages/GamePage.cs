using Nokemono.Core.UI.ViewModels;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Input;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;
using Ssit.CrossX.UI.Views;

namespace Nokemono.Core.UI.Pages;

public class GamePage: Page<GamePageViewModel>
{
    private IUiSounds _gameUiSounds;
    private IInputContext _inputContext;
    
    private float _showPaletteNameTime = 0;
    private SharedStringValue _paletteName = new();
    
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

    protected override void OnUpdate(float dt)
    {
        base.OnUpdate(dt);

        if (Services.Get<IKeyboard>().GetKey(Key.RightBracket) == ButtonState.JustPressed
            || (Services.Get<IGameControllers>().GetButton(0, GameControllerButton.RightShoulder) == ButtonState.JustPressed &&
                Services.Get<IGameControllers>().GetButton(0, GameControllerButton.Back).IsDown))
        {
            var settings = Services.Get<ISettingsProvider>().Settings;
            settings.Palette = (settings.Palette + 1) % Palette.Palettes.Length;
            settings.Save();
            
            _paletteName.SetText(Palette.Palettes[settings.Palette].Name);
            _showPaletteNameTime = 1.25f;
        }
        
        if (Services.Get<IKeyboard>().GetKey(Key.LeftBracket) == ButtonState.JustPressed
            || (Services.Get<IGameControllers>().GetButton(0, GameControllerButton.LeftShoulder) == ButtonState.JustPressed &&
                Services.Get<IGameControllers>().GetButton(0, GameControllerButton.Back).IsDown))
        {
            var settings = Services.Get<ISettingsProvider>().Settings;
            settings.Palette = (settings.Palette + Palette.Palettes.Length - 1) % Palette.Palettes.Length;
            settings.Save();
            
            _paletteName.SetText(Palette.Palettes[settings.Palette].Name);
            _showPaletteNameTime = 1.25f;
        }

        if (_showPaletteNameTime > 0)
        {
            _showPaletteNameTime -= dt;
            if (_showPaletteNameTime < 0)
            {
                _showPaletteNameTime = 0;
                _paletteName.SetText("");
            }
        }
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
                    Text = _paletteName,
                    AnchorX = "100%-10",
                    AnchorY = 10,
                    HorizontalAlign  = Align.End,
                    VerticalAlign = Align.Start,
                    TextAlign = ContentAlign.Right,
                    Font = ("Default", 8),
                    Scaling = TextScaling.Default,
                    TextColor = Palette.Foreground,
                    TextOutlineColor = Palette.Background,
                },
                new Label
                {
                    Text = ViewModel.Fps,
                    AnchorX = 4,
                    AnchorY = 4,
                    VerticalAlign = Align.Start,
                    HorizontalAlign = Align.Start,
                    TextAlign = ContentAlign.Left | ContentAlign.Top,
                    Font = ("Default", 8),
                    TextColor = Palette.Foreground,
                    TextOutlineColor = Palette.Background,
                    Scaling = TextScaling.Default,
                    Visible = ViewModel.ShowFps
                }
            ]
        };
    }
}