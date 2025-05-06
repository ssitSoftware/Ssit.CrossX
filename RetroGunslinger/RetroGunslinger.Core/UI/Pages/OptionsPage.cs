using RetroGunslinger.Core.UI.ViewModels;
using Ssit.CrossX.UI.Views;

namespace RetroGunslinger.Core.UI.Pages;

public class OptionsPage : OptionsPageBase<OptionsPageViewModel>
{
    protected override View CreateView()
    {
        return CreateDefaultItemsContainer(CreateMenu());
    }
}