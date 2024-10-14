using System.Threading.Tasks;

namespace Ssit.Utils.Mvvm.Abstractions;

/// <summary>
/// Represents a service for displaying dialogs.
/// </summary>
public interface IDialogService
{
    /// <summary>
    /// Displays a dialog with the specified view model.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <param name="parameter">An optional parameter to pass to the view model constructor.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task ShowDialog<TViewModel>(object parameter = null);
}