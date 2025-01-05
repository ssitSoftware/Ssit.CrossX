using SampleGame.Game.UI.ViewModels;
using Ssit.CrossX.UI.Views;

namespace SampleGame.Game.UI.Pages;

public class MainPage: MenuItemsPageBase<MainPageViewModel>
{
    protected override View CreateView()
    {
        var menuView = CreateMenuItems("MainMenu",
        [
            ("StartGame", ViewModel.StartGameCommand),
            ("Options", ViewModel.OptionsCommand),
            ("Credits", ViewModel.CreditsCommand),
            ("Exit", ViewModel.ExitCommand)
        ], 0, true);

        return CreateDefaultItemsContainer(menuView);
    }
}