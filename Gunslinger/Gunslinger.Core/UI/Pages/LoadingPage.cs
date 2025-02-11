using Gunslinger.Core.UI.ViewModels;
using Ssit.CrossX;
using Ssit.CrossX.Common.Services;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Views;

namespace Gunslinger.Core.UI.Pages;

internal class LoadingPage: Page<LoadingPageViewModel>
{
    protected override View CreateView()
    {
        return new Container
        {
            BackgroundColor = RgbaColor.Black,
            Children = []
        };
    }

    private ITranslator Translator => Services.Get<ITranslator>();
}