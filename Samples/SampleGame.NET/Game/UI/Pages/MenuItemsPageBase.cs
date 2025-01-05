using System.Collections.Generic;
using System.Windows.Input;
using SampleGame.Game.UI.Styles;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;
using Ssit.CrossX.UI.Views;

namespace SampleGame.Game.UI.Pages;

public abstract class MenuItemsPageBase<TViewModel>: Page<TViewModel> where TViewModel: class
{
    private string _defaultId;
    
    protected override bool OnUiButton(UiButton button, IInputContext inputContext)
    {
        if (FocusedElement is null && !string.IsNullOrWhiteSpace(_defaultId))
        {
            var focusable = inputContext.FindFocusable(_defaultId, this);
            inputContext.Focus(focusable, this);
            return true;
        }
        return base.OnUiButton(button, inputContext);
    }

    protected View CreateMenuItems(string id, IReadOnlyList<(SharedString text, ICommand command)> items,
        int defaultButtonIndex = -1, bool cycleItems = false)
    {
        var buttons = new List<View>();
        for (var i = 0; i < items.Count; i++)
        {
            var nextIndex = i + 1;
            var prevIndex = i - 1;

            if (cycleItems)
            {
                nextIndex %= items.Count;
                prevIndex = (prevIndex + items.Count) % items.Count;
            }
            
            var button = new LabelButton
            {
                Text = items[i].text,
                UniqueId = $"{id}{i}",
                VerticalNavigation = ($"{id}{prevIndex}", $"{id}{nextIndex}"),
                Command = items[i].command
            }.WithDefaultStyle();
            
            buttons.Add(button);
        }

        if (defaultButtonIndex >= 0)
        {
            _defaultId = $"{id}{defaultButtonIndex}";
        }

        return new VerticalStack
        {
            Padding = (8, 8),
            Spacing = 8,
            VerticalAlign = Align.Center,
            HorizontalAlign = Align.Center,
            Width = "192",
            Children = buttons.ToArray()
        };
    }

    protected View CreateDefaultItemsContainer(View itemsView)
    {
        return new Container
        {
            Width = 480,
            HorizontalAlign = Align.Center,
            Padding = (10,10),
            Children = [
                itemsView
            ]
        };
    }
}