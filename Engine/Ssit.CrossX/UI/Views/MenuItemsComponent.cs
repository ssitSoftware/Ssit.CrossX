using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Ssit.CrossX.UI.Common.Services;
using Ssit.CrossX.UI.Handlers;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;

namespace Ssit.CrossX.UI.Views;

public class MenuItemsComponent<TButton, TVerticalStack>: VerticalStack where TButton : LabelButton, new() where TVerticalStack: VerticalStack, new()
{
    public MenuItemsComponent()
    {
        Padding = (8, 8);
        VerticalAlign = Align.Center;
        HorizontalAlign = Align.Center;
        
        CustomHandlerType = typeof(VerticalStackHandler<MenuItemsComponent<TButton, TVerticalStack>>);
    }
    
    public string IdPrefix { get; set; }
    
    public IReadOnlyList<(SharedString text, ICommand command, bool enableCommandType)> ItemsWithCommandType { get; set; }

    public IReadOnlyList<(SharedString text, ICommand command)> Items
    {
        set
        {
            ItemsWithCommandType = value.Select(i => (i.text, i.command, false)).ToArray();
        }
    }
    
    public IPageCommandsSource CommandsSource { get; set; }

    public Action<TButton> ApplyButtonStyle { get; set; }
    
    public string GenerateDefaultId(int defaultButtonIndex = 0)
    {
        return $"{IdPrefix}{defaultButtonIndex}";
    }
    
    protected override void Initialize(IUiServices services)
    {
        var translator = services.IoCContainer.Get<ITranslator>();
        var controls = new List<View>();
        var commandsSource = CommandsSource;
        
        int navId = 0;
        int navCount = ItemsWithCommandType.Count(o=>o.text != null);
        
        for (var i = 0; i < ItemsWithCommandType.Count; i++)
        {
            var item = ItemsWithCommandType[i];
            if ((item.text?.Length ?? 0) == 0)
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
                Text = item.text,
                UniqueId = $"{IdPrefix}{navId}",
                VerticalNavigation = ($"{IdPrefix}{prevIndex}", $"{IdPrefix}{nextIndex}"),
                Command = item.command,
                EnableCommandType = item.enableCommandType
            };
            ApplyButtonStyle?.Invoke(button);
            controls.Add(button);
            navId++;
        }

        if (commandsSource is not null && commandsSource.BackCommand != null)
        {
            controls.Add(new Background
            {
                Height = 4
            });

            var button = new TButton
            {
                Text = translator["Back"],
                UniqueId = $"{IdPrefix}{navCount}",
                VerticalNavigation = ($"{IdPrefix}{navCount - 1}", $"{IdPrefix}0"),
                Command = commandsSource.BackCommand
            };

            ApplyButtonStyle?.Invoke(button);
            controls.Add(button);
        }
        
        Children = controls.ToArray();
    }
}