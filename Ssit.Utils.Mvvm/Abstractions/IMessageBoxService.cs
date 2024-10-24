using System.Threading.Tasks;

namespace Ssit.Utils.Mvvm.Abstractions;

/// <summary>
/// Represents a service for displaying message boxes.
/// </summary>
public interface IMessageBoxService
{
    /// <summary>
    /// Displays a message box to the user with specified parameters.
    /// </summary>
    /// <param name="title">The title of the message box.</param>
    /// <param name="message">The message text to display.</param>
    /// <param name="buttons">The buttons to include in the message box.</param>
    /// <param name="icon">The icon to display in the message box (default is none).</param>
    /// <returns>A task that represents the result of the message box interaction, returning the button clicked by the user.</returns>
    Task<MessageBoxButton> ShowMessageBox(string title, string message, MessageBoxButton buttons, MessageBoxIcon icon = MessageBoxIcon.None);
}