using Nokemono.Core.UI.Pages.Internal;
using Nokemono.Core.UI.ViewModels;
using Nokemono.Core.UI.Views;
using Ssit.CrossX.UI.Views;

namespace Nokemono.Core.UI.Pages;

public abstract class OptionsPageBase<TViewModel>: MenuItemsPageBaseEx<TViewModel> where TViewModel: OptionsPageViewModel
{
    protected View CreateMenu()
    {
        var menuView = CreateMenuItems<LabelButtonEx>("Options",
        [
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