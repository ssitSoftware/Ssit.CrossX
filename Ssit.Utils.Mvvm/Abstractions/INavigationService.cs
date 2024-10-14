using System.Threading.Tasks;

namespace Ssit.Utils.Mvvm.Abstractions;

/// <summary>
/// Provides navigation services for navigating between view models.
/// </summary>
public interface INavigationService
{
    /// <summary>
    /// Navigates to the view/page corresponding to specified view model with an optional parameter for its creation.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the view model to navigate to.</typeparam>
    /// <param name="parameter">An optional parameter to pass to the view model.</param>
    /// <returns>A task representing the asynchronous navigation operation.</returns>
    Task NavigateTo<TViewModel>(object parameter = null);

    /// <summary>
    /// Clears the navigation stack and navigates to the view/page corresponding to specified view model with an optional parameter for its creation.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the view model to navigate to.</typeparam>
    /// <param name="parameter">An optional parameter to pass to the view model.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ClearTo<TViewModel>(object parameter = null);

    /// <summary>
    /// Navigates back to the previous view or page.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task NavigateBack();

    /// <summary>
    /// Navigates back to the view/page with specified ViewModel in the navigation stack.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the ViewModel to navigate back to.</typeparam>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task NavigateBackTo<TViewModel>();
}