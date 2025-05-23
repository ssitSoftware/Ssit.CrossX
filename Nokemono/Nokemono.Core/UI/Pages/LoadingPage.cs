using Nokemono.Core.UI.ViewModels;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Views;

namespace Nokemono.Core.UI.Pages;

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