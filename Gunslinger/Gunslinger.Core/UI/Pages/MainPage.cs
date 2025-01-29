using Gunslinger.Core.UI.Pages.Internal;
using Gunslinger.Core.UI.ViewModels;
using Ssit.CrossX.UI.Views;

namespace Gunslinger.Core.UI.Pages;

internal class MainPage: MenuItemsPageBaseEx<MainPageViewModel>
{
    protected override View CreateView()
    {
        var menuView = CreateMenuItems("MainMenu",
        [
            (Translator["Start Game"], ViewModel.StartGameCommand),
            (Translator["Language"] + ": " + Translator["#LangName"], ViewModel.LanguageCommand),
            (Translator["Options"], ViewModel.OptionsCommand),
            (Translator["Credits"], ViewModel.CreditsCommand)
        ]);

        return CreateDefaultItemsContainer(menuView);
    }
}