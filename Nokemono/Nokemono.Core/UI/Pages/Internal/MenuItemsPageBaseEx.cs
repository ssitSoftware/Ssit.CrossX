using Nokemono.Core.UI.Styles;
using Nokemono.Core.UI.Views;
using Ssit.CrossX.Common.Pages;
using Ssit.CrossX.UI.Views;

namespace Nokemono.Core.UI.Pages.Internal;

public abstract class MenuItemsPageBaseEx<TViewModel> : MenuItemsPageBase<TViewModel> where TViewModel : class
{
    protected override void MenuItemApplyStyle(LabelButton button)
    {
        button.WithDefaultStyle();

        if (button is LabelButtonEx labelButtonEx)
        {
            labelButtonEx.FocusWaveAmplitude = (labelButtonEx.Font?.FontSize ?? 12) / 9;
            labelButtonEx.FocusWaveFrequency = 1f;
            labelButtonEx.FocusBevel = (labelButtonEx.Font?.FontSize ?? 12) / 8;
        }
    }

    protected override void MenuApplyStyle(VerticalStack stack)
    {
        base.MenuApplyStyle(stack);
        stack.Spacing = 4;
    }
}