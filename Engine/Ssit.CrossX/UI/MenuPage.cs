using Ssit.CrossX.UI.Common.Pages;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;

namespace Ssit.CrossX.UI;

public abstract class MenuPage<TViewModel>(float transitionTime = 0.3f) : PageWithTranslator<TViewModel> where TViewModel: class
{
    public override float TransitionTime => transitionTime;
    
    protected string DefaultId { get; set; }

    protected override void OnLoad(IInputContext inputContext)
    {
        if (Services.Get<PageInputContext>().ShowFocus)
        {
            var focusable = inputContext.FindFocusable(DefaultId, this);
            inputContext.Focus(focusable, this);
        }
    }
    
    protected override bool OnUiButton(UiButton button, IInputContext inputContext)
    {
        if (FocusedElement is null && !string.IsNullOrWhiteSpace(DefaultId) && button is not UiButton.Back and not UiButton.MenuOrBack)
        {
            var focusable = inputContext.FindFocusable(DefaultId, this);
            inputContext.Focus(focusable, this);
            Services.Get<PageInputContext>().ShowFocus = true;
            return true;
        }
        return base.OnUiButton(button, inputContext);
    }
}