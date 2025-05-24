using Nokemono.Core.UI.Styles;
using Nokemono.Core.UI.Views;
using Ssit.CrossX.UI.Common.Pages;
using Ssit.CrossX.UI.Views;

namespace Nokemono.Core.UI.Pages.Internal;

public abstract class MenuItemsPageBaseEx<TViewModel> : MenuItemsPageBase<TViewModel> where TViewModel : class
{
    protected override void MenuItemApplyStyle(LabelButton button)
    {
        (button as LabelButtonEx)?.WithDefaultStyle();
    }

    protected override void MenuApplyStyle(VerticalStack stack)
    {
        base.MenuApplyStyle(stack);
        stack.Spacing = 4;
    }
}