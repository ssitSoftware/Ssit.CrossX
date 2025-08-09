using System.Numerics;
using Nokemono.Core.UI.ViewModels;
using Ssit.CrossX.UI.Transitions;
using Ssit.CrossX.UI.Views;

namespace Nokemono.Core.UI.Pages;

public class OptionsPageInGame : OptionsPageBase<OptionsPageInGameViewModel>
{
    protected override View CreateView()
    {
        TransitionTime = 0.3f;
        
        var menuView = CreateMenu();
        
        menuView.Transitions =
        [
            new TranslationTransition
            {
                ForTransitions = TransitionType.Navigation,
                Offset = new Vector2(0, 270),
                Power = 3,
                ProgressMin = 0.2f
            }
        ];
        
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