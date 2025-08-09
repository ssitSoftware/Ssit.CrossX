using System.Numerics;
using Nokemono.Core.UI.ViewModels;
using Ssit.CrossX.UI.Transitions;
using Ssit.CrossX.UI.Views;

namespace Nokemono.Core.UI.Pages;

public class OptionsPage : OptionsPageBase<OptionsPageViewModel>
{
    protected override View CreateView()
    {
        TransitionTime = 0.2f;
        
        var menu = CreateMenu();
        menu.Transitions =
        [
            new TranslationTransition
            {
                ForTransitions = TransitionType.Navigation,
                Offset = new Vector2(0, 270),
                Power = 3,
                ProgressMin = 0.2f
            }
        ];
        return CreateDefaultItemsContainer(menu);
    }
}