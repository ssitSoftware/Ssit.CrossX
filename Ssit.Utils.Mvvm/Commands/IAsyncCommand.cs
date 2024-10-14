using System.Threading.Tasks;

namespace Ssit.Utils.Mvvm.Commands;

/// <summary>
/// Represents an asynchronous command interface that extends from the ISyncCommand interface.
/// </summary>
public interface IAsyncCommand : ISyncCommand
{
    /// <summary>
    /// Executes the command asynchronously with the given parameter.
    /// </summary>
    /// <param name="parameter">The parameter to pass to the command's action.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ExecuteAsync(object parameter = null);
}