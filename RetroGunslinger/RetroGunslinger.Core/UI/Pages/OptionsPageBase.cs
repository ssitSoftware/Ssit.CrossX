using RetroGunslinger.Core.UI.Pages.Internal;
using RetroGunslinger.Core.UI.ViewModels;
using RetroGunslinger.Core.UI.Views;
using Ssit.CrossX.UI.Views;

namespace RetroGunslinger.Core.UI.Pages;

public abstract class OptionsPageBase<TViewModel>: MenuItemsPageBaseEx<TViewModel> where TViewModel: OptionsPageViewModel
{
    protected View CreateMenu()
    {
        var menuView = CreateMenuItems<LabelButtonEx>("Options",
        [
            //(Translator["Fullscreen"] +": " + ViewModel.FullscreenStr, ViewModel.FullScreenCommand, true),
            //(Translator["Scale"] +": " + ViewModel.ScaleStr, ViewModel.ScaleCommand, true),
            (Translator["Sound Volume"] +": " + ViewModel.SoundVolumeStr, ViewModel.SoundVolumeCommand, true),
            (Translator["Music Volume"] + ": "+ ViewModel.MusicVolumeStr, ViewModel.MusicVolumeCommand, true),
            (null, null, false),
            (Translator["Language"] + ": " + Translator["#LangName"], ViewModel.LanguageCommand, true),
            (null, null, false),
            (Translator["CrtMode"] + ": " + ViewModel.CrtStr, ViewModel.CrtCommand, true),
            (Translator["Palette"] + ": " + ViewModel.PaletteStr, ViewModel.PaletteCommand, true),
        ]);
        return menuView;
    }
}