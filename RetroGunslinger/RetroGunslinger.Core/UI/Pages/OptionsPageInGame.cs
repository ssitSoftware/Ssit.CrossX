using RetroGunslinger.Core.UI.ViewModels;
using Ssit.CrossX;
using Ssit.CrossX.UI.Views;

namespace RetroGunslinger.Core.UI.Pages;

public class OptionsPageInGame : OptionsPageBase<OptionsPageInGameViewModel>
{
    protected override View CreateView()
    {
        var menuView = CreateMenu();

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