using SampleGame.UI.Pages.Internal;
using Ssit.CrossX.UI.Views;

namespace SampleGame.UI.Pages;

public class MainPage: MenuItemsPageBase<MainPageViewModel>
{
    protected override View CreateView()
    {
        var menuView = CreateMenuItems("MainMenu",
        [
            (Translator["Start Game"], ViewModel.StartGameCommand),
            (Translator["Language"] + ": " + Translator["#LangName"], ViewModel.LanguageCommand),
            (Translator["Options"], ViewModel.OptionsCommand),
            (Translator["Credits"], ViewModel.CreditsCommand),
            (Translator["Exit"], ViewModel.ExitCommand)
        ]);

        return CreateDefaultItemsContainer(menuView);
    }
}