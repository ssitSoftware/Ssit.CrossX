using RetroGunslinger.Core.UI.Pages.Internal;
using RetroGunslinger.Core.UI.ViewModels;
using RetroGunslinger.Core.UI.Views;
using Ssit.CrossX;
using Ssit.CrossX.UI.Views;

namespace RetroGunslinger.Core.UI.Pages;

public class PausePage: MenuItemsPageBaseEx<PausePageViewModel>
{
    protected override View CreateView()
    {
        var menuView = CreateMenuItems<LabelButtonEx>("Pause",
        [
            (Translator["Resume"], ViewModel.ResumeCommand),
            (Translator["Options"], ViewModel.OptionsCommand),
            (Translator["Exit to Main Menu"], ViewModel.ExitCommand)
        ], suppressBack: true);
        
        return new Container
        {
            Children = [
                new GameView
                {
                    GameInstance = ViewModel.GameInstance,
                    Active = false
                },
                new Background
                {
                    // TODO: Mask image for pause screen
                    BackgroundColor = RgbaColor.Black * 0.5f
                },
                new Container
                {
                    Padding = (10,10),
                    Children = [menuView]
                }
            ]
        };
    }
}