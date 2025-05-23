using Nokemono.Core.UI.ViewModels;
using Ssit.CrossX.UI.Views;

namespace Nokemono.Core.UI.Pages;

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
                    GameInstance = ViewModel.GameInterfaces.Instance,
                    Active = false
                },
                DialogPageHelper.CreateDialogLayer(ViewModel.GameInterfaces.Dialogs, false),
                new Background
                {
                    BackgroundColor = (1, 0.9f)
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