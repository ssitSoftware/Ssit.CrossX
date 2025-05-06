using RetroGunslinger.Core.UI.ViewModels;
using Ssit.CrossX;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Views;

namespace RetroGunslinger.Core.UI.Pages;

internal class LoadingPage: Page<LoadingPageViewModel>
{
    protected override View CreateView()
    {
        return new Container
        {
            BackgroundColor = Palette.Background,
            Children = []
        };
    }
}