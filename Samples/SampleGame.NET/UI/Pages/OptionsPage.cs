using SampleGame.UI.Pages.Internal;
using SampleGame.UI.Styles;
using Ssit.CrossX.Common.Pages;
using Ssit.CrossX.UI.Views;

namespace SampleGame.UI.Pages;

public class OptionsPage: MenuItemsPageBase<OptionsPageViewModel>
{
    protected override View CreateView()
    {
        var menuView = CreateMenuItems("Options",
        [
            (Translator["Sound Volume"] +": " + ViewModel.SoundVolumeStr, ViewModel.SoundVolumeCommand),
            (Translator["Music Volume"] + ": "+ ViewModel.MusicVolumeStr, ViewModel.MusicVolumeCommand),
            (Translator["Camera Shake"] + ": " + ViewModel.CameraShakeStr, ViewModel.CameraShakeCommand),
            (Translator["Auto-Reload"] + ": " + ViewModel.AutoReloadStr, ViewModel.AutoReloadCommand),
            (Translator["Controls"], ViewModel.ControlsCommand)
        ]);

        return CreateDefaultItemsContainer(menuView);
    }
    
    protected override void MenuItemApplyStyle(LabelButton button)
    {
        button.WithDefaultStyle();
    }
}