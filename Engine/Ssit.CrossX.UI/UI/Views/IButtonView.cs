using System.Windows.Input;

namespace Ssit.CrossX.UI.Views;

public interface IButtonView
{
    ICommand Command { get; }
    object CommandParameter { get; }
    
    (string left, string right) HorizontalNavigation { get; set; }
    (string up, string down) VerticalNavigation { get; set; }
}