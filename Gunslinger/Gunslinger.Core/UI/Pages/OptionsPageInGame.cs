using Gunslinger.Core.UI.ViewModels;
using Ssit.CrossX;
using Ssit.CrossX.Common.Views;
using Ssit.CrossX.UI.Views;

namespace Gunslinger.Core.UI.Pages;

public class OptionsPageInGame : OptionsPageBase<OptionsPageInGameViewModel>
{
    protected override View CreateView()
    {
        var menuView = CreateMenu();

        return new Container
        {
            Children = [
                new GameView
                {
                    Simulation = ViewModel.Simulation,
                    Active = false
                },
                new Background
                {
                    BackgroundColor = RgbaColor.Black * 0.5f
                },
                new Container
                {
                    Padding = (10,10),
                    Children = [menuView]
                }
            ]
        };
    }
}