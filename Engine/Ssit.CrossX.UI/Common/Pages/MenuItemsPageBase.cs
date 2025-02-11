using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.Common.Pages;

public abstract class MenuItemsPageBase<TViewModel>: PageWithTranslator<TViewModel> where TViewModel: class
{
    private string _defaultId;

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

    protected View CreateMenuItems<TButton>(string id, IReadOnlyList<(SharedString text, ICommand command)> items,
        int defaultButtonIndex = 0, bool isMainList = true, bool suppressBack = false) where TButton: LabelButton, new()
    {
        var controls = new List<View>();
        var commandsSource = isMainList ? ViewModel as IPageCommandsSource : null;

        int navId = 0;
        int navCount = items.Count(o=>o.text != null);
        
        for (var i = 0; i < items.Count; i++)
        {
            if (items[i].text == null)
            {
                controls.Add(new Background
                {
                    Height = 4
                });
                continue;
            }
            
            var nextIndex = navId + 1;
            var prevIndex = navId - 1;
            
            if (commandsSource is null)
            {
                nextIndex %= navCount;
                prevIndex = (prevIndex + navCount) % navCount;
            }
            else if(prevIndex < 0)
            {
                prevIndex = navCount;
            }

            var button = new TButton
            {
                Text = items[i].text,
                UniqueId = $"{id}{navId}",
                VerticalNavigation = ($"{id}{prevIndex}", $"{id}{nextIndex}"),
                Command = items[i].command
            };
            MenuItemApplyStyle(button);
            controls.Add(button);
            navId++;
        }

        if (!suppressBack && commandsSource is not null && commandsSource.BackCommand != null)
        {
            controls.Add(new Background
            {
                Height = 4
            });

            var button = new TButton
            {
                Text = Translator["Back"],
                UniqueId = $"{id}{navCount}",
                VerticalNavigation = ($"{id}{navCount - 1}", $"{id}0"),
                Command = commandsSource.BackCommand
            };

            MenuItemApplyStyle(button);
            controls.Add(button);
        }

        if (defaultButtonIndex >= 0)
        {
            _defaultId = $"{id}{defaultButtonIndex}";
        }

        var stack = CreateVerticalStack();
        stack.Children = controls.ToArray();

        MenuApplyStyle(stack);
        return stack;
    }

    protected virtual VerticalStack CreateVerticalStack()
    {
        return new VerticalStack
        {
            Padding = (8, 8),
            VerticalAlign = Align.Center,
            HorizontalAlign = Align.Fill
        };
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