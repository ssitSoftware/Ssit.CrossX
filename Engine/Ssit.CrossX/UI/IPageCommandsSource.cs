using System.Windows.Input;

namespace Ssit.CrossX.UI;

public interface IPageCommandsSource
{
    ICommand BackCommand => null;
    ICommand MenuCommand => null;
}