using System.Collections.Generic;
using System.Windows.Input;
using SampleGame.Services;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.Common.Pages;

public abstract class MenuItemsPageBase<TViewModel>: Page<TViewModel> where TViewModel: class
{
    private string _defaultId;
    protected ITranslator Translator => Services.Get<ITranslator>();

    protected abstract void MenuItemApplyStyle(LabelButton button);
    
    protected virtual void MenuApplyStyle(VerticalStack stack)
    {
    }
    
    protected override void OnLoad(IInputContext inputContext)
    {
        base.OnLoad(inputContext);
        
        if (Services.Get<PageInputContext>().AlwaysShowFocus)
        {
            var focusable = inputContext.FindFocusable(_defaultId, this);
            inputContext.Focus(focusable, this);
        }
    }

    protected override bool OnUiButton(UiButton button, IInputContext inputContext)
    {
        if (FocusedElement is null && !string.IsNullOrWhiteSpace(_defaultId) && button is not UiButton.Back and not UiButton.MenuOrBack)
        {
            var focusable = inputContext.FindFocusable(_defaultId, this);
            inputContext.Focus(focusable, this);
            Services.Get<PageInputContext>().AlwaysShowFocus = true;
            return true;
        }
        return base.OnUiButton(button, inputContext);
    }

    protected View CreateMenuItems(string id, IReadOnlyList<(SharedString text, ICommand command)> items,
        int defaultButtonIndex = 0, bool isMainList = true)
    {
        var controls = new List<View>();
        var commandsSource = isMainList ? ViewModel as IPageCommandsSource : null;
            
        for (var i = 0; i < items.Count; i++)
        {
            var nextIndex = i + 1;
            var prevIndex = i - 1;

            if (commandsSource is null)
            {
                nextIndex %= items.Count;
                prevIndex = (prevIndex + items.Count) % items.Count;
            }
            else if(prevIndex < 0)
            {
                prevIndex = items.Count;
            }

            var button = new LabelButton
            {
                Text = items[i].text,
                UniqueId = $"{id}{i}",
                VerticalNavigation = ($"{id}{prevIndex}", $"{id}{nextIndex}"),
                Command = items[i].command
            };
            MenuItemApplyStyle(button);
            controls.Add(button);
        }

        if (commandsSource is not null)
        {
            controls.Add(new Background
            {
                Height = 4
            });

            var button = new LabelButton
            {
                Text = "Back",
                UniqueId = $"{id}{items.Count}",
                VerticalNavigation = ($"{id}{items.Count - 1}", $"{id}0"),
                Command = commandsSource.BackCommand
            };

            MenuItemApplyStyle(button);
            controls.Add(button);
        }

        if (defaultButtonIndex >= 0)
        {
            _defaultId = $"{id}{defaultButtonIndex}";
        }

        var stack = new VerticalStack
        {
            BackgroundColor = RgbaColor.FromNonPremultiplied(0, 0, 0, 100),
            Padding = (8, 8),
            Spacing = 8,
            VerticalAlign = Align.Center,
            HorizontalAlign = Align.Center,
            Width = 192,
            Children = controls.ToArray()
        };

        MenuApplyStyle(stack);
        return stack;
    }

    protected View CreateDefaultItemsContainer(View itemsView, Thickness? padding = null)
    {
        return new Container
        {
            Padding = padding ?? (10,10),
            Children = [
                itemsView
            ]
        };
    }
}