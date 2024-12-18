using System.Windows.Input;

namespace Ssit.CrossX.UI;

public interface IBackCommandSource
{
    ICommand BackCommand { get; }
}