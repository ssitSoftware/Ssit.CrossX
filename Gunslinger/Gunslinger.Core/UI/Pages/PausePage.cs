using Gunslinger.Core.UI.Pages.Internal;
using Gunslinger.Core.UI.ViewModels;
using Gunslinger.Core.UI.Views;
using Ssit.CrossX;
using Ssit.CrossX.UI.Views;

namespace Gunslinger.Core.UI.Pages;

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