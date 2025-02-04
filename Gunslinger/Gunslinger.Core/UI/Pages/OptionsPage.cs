using Gunslinger.Core.UI.Pages.Internal;
using Gunslinger.Core.UI.ViewModels;
using Gunslinger.Core.UI.Views;
using Ssit.CrossX.UI.Views;

namespace Gunslinger.Core.UI.Pages;

public class OptionsPage: MenuItemsPageBaseEx<OptionsPageViewModel>
{
    protected override View CreateView()
    {
        var menuView = CreateMenuItems<LabelButtonEx>("Options",
        [
            (Translator["Sound Volume"] +": " + ViewModel.SoundVolumeStr, ViewModel.SoundVolumeCommand),
            (Translator["Music Volume"] + ": "+ ViewModel.MusicVolumeStr, ViewModel.MusicVolumeCommand),
            (null, null),
            (Translator["Language"] + ": " + Translator["#LangName"], ViewModel.LanguageCommand),
            (null, null),
            (Translator["Camera Shake"] + ": " + ViewModel.CameraShakeStr, ViewModel.CameraShakeCommand),
            (Translator["Controls"], ViewModel.ControlsCommand)
        ]);

        return CreateDefaultItemsContainer(menuView);
    }
}