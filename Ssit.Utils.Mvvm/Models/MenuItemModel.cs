using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Ssit.Utils.Mvvm.Models;

/// <summary>
/// Represents a menu item model used in the User Interface.
/// </summary>
public class MenuItemModel
{
    public string Header { get; }
    public ICommand Command { get; }
    public object CommandParameter { get; }

    public IList<MenuItemModel> Items { get; }

    public MenuItemModel(string header, ICommand command, object commandParameter = null)
    {
        Header = header;
        Command = command;
        CommandParameter = commandParameter;
    }

    public MenuItemModel(string header, IEnumerable<MenuItemModel> subItems)
    {
        Header = header;
        Items = subItems?.ToArray();
    }

    public MenuItemModel()
    {
        Header = "-";
    }
}