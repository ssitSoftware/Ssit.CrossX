using System.Threading.Tasks;
using System.Windows.Input;

namespace Ssit.CrossX.Commands;

public interface IAsyncCommand : ICommand
{
    Task ExecuteAsync(object parameter);
}