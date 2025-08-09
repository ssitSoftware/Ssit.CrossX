using System.Numerics;
using Nokemono.Core.UI.Pages.Internal;
using Nokemono.Core.UI.ViewModels;
using Nokemono.Core.UI.Views;
using Ssit.CrossX.UI.Transitions;
using Ssit.CrossX.UI.Views;

namespace Nokemono.Core.UI.Pages;

public class PausePage: MenuItemsPageBaseEx<PausePageViewModel>
{
    protected override View CreateView()
    {
        TransitionTime = 0.15f;
        
        var menuView = CreateMenuItems<LabelButtonEx>("Pause",
        [
            (Translator["Resume"], ViewModel.ResumeCommand),
            (Translator["Options"], ViewModel.OptionsCommand),
            (Translator["Exit to Main Menu"], ViewModel.ExitCommand)
        ], suppressBack: true);
        
        menuView.Transitions =
        [
            new TranslationTransition
            {
                ForTransitions = TransitionType.NavigateFrom | TransitionType.NavigateBackTo,
                Offset = new Vector2(-360, 0),
                Power = 2
            },
            new HideTransition
            {
                ForTransitions = TransitionType.NavigateBackFrom
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
                    BackgroundColor = (1, 0.9f),
                    Transitions = [
                        new HideTransition
                        {
                            ForTransitions = TransitionType.NavigateBackFrom
                        }]
                },
                new Container
                {
                    Padding = (10,10),
                    Children = [menuView]
                }
            ]
        };
    }

    // protected override VerticalStack CreateVerticalStack()
    // {
    //     return new VerticalStack
    //     {
    //         Padding = (8, 8),
    //         VerticalAlign = Align.Center,
    //         HorizontalAlign = Align.Center,
    //         BackgroundColor = 1
    //     };
    // }
}