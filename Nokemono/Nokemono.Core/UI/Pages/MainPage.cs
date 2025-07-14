using Nokemono.Core.UI.Pages.Internal;
using Nokemono.Core.UI.ViewModels;
using Nokemono.Core.UI.Views;
using Ssit.CrossX.Audio;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;
using Ssit.CrossX.UI.Views;

namespace Nokemono.Core.UI.Pages;

internal class MainPage: MenuItemsPageBaseEx<MainPageViewModel>
{
    protected override void OnLoad(IInputContext inputContext)
    {
        base.OnLoad(inputContext);
        Services.Get<IMusicPlayer>().ChangePlaylist("Menu");
    }
    
    protected override View CreateView()
    {
        var menuView = CreateMenuItems<LabelButtonEx>("MainMenu",
        [
            (Translator["Start Game"], ViewModel.StartGameCommand),
            (Translator["Options"], ViewModel.OptionsCommand),
            (Translator["Credits"], ViewModel.OptionsCommand),
            (Translator["Exit"], ViewModel.ExitCommand)
        ]);

        return new Container
        {
            Children = [
                new ImageView
                {
                  Source  = "assets:/UI/Logo.png!",
                  AnchorX = "50%",
                  HorizontalAlign = Align.Center,
                  VerticalAlign = Align.Start,
                  AnchorY = 10,
                  Scaling = ImageScalingMode.None,
                  Width = Length.Auto,
                  Height = Length.Auto
                },
                new Label
                {
                    Text = Translator["THE LOST BARRELS"],
                    HorizontalAlign = Align.End,
                    TextAlign = ContentAlign.Right,
                    VerticalAlign = Align.Start,
                    AnchorY = 70,
                    AnchorX = "84%",
                    Font = ("Default", 12),
                    TextColor = Palette.Foreground,
                    TextOutlineColor = Palette.Background,
                    Scaling = TextScaling.Pixel
                },
                menuView,
                new Label
                {
                    Text = "© 2025 ebatianoGames Sebastian Sejud™.\n" + Translator["All rights reserved."],
                    AnchorX = "50%",
                    AnchorY = "100%-8",
                    HorizontalAlign = Align.Center,
                    VerticalAlign = Align.End,
                    Font = ("Default", 12),
                    TextColor = Palette.Dim,
                    TextOutlineColor = Palette.Background,
                    Scaling = TextScaling.Pixel
                }
            ]
        };
    }

    protected override void MenuItemApplyStyle(LabelButton button)
    {
        base.MenuItemApplyStyle(button);
        
        button.Font = ("Default", 22);
        button.Height = 26;
    }

    protected override void MenuApplyStyle(VerticalStack stack)
    {
        base.MenuApplyStyle(stack);
        stack.AnchorY = "50%+25";
    }
}