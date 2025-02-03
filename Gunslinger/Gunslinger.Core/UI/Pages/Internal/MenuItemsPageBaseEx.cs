using Gunslinger.Core.UI.Styles;
using Ssit.CrossX.Common.Pages;
using Ssit.CrossX.UI.Views;

namespace Gunslinger.Core.UI.Pages.Internal;

public abstract class MenuItemsPageBaseEx<TViewModel> : MenuItemsPageBase<TViewModel> where TViewModel : class
{
    protected override void MenuItemApplyStyle(LabelButton button)
    {
        button.WithDefaultStyle();
    }

    protected override void MenuApplyStyle(VerticalStack stack)
    {
        base.MenuApplyStyle(stack);
        stack.Spacing = 4;
    }
}