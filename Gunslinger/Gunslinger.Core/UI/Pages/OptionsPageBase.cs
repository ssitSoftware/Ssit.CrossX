using Gunslinger.Core.UI.Pages.Internal;
using Gunslinger.Core.UI.ViewModels;
using Gunslinger.Core.UI.Views;
using Ssit.CrossX.UI.Views;

namespace Gunslinger.Core.UI.Pages;

public abstract class OptionsPageBase<TViewModel>: MenuItemsPageBaseEx<TViewModel> where TViewModel: OptionsPageViewModel
{
    protected View CreateMenu()
    {
        var menuView = CreateMenuItems<LabelButtonEx>("Options",
        [
            (Translator["Fullscreen"] +": " + ViewModel.FullscreenStr, ViewModel.FullScreenCommand),
            (Translator["Scale"] +": " + ViewModel.ScaleStr, ViewModel.ScaleCommand),
            (Translator["Sound Volume"] +": " + ViewModel.SoundVolumeStr, ViewModel.SoundVolumeCommand),
            (Translator["Music Volume"] + ": "+ ViewModel.MusicVolumeStr, ViewModel.MusicVolumeCommand),
            (null, null),
            (Translator["Language"] + ": " + Translator["#LangName"], ViewModel.LanguageCommand),
            (null, null),
            (Translator["Camera Shake"] + ": " + ViewModel.CameraShakeStr, ViewModel.CameraShakeCommand),
            (Translator["Controls"], ViewModel.ControlsCommand)
        ]);
        return menuView;
    }
}