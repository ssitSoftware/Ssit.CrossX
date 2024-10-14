using System.Windows.Input;

namespace Ssit.Utils.Mvvm.Commands;

/// <summary>
/// Represents a synchronous command interface that extends System.Windows.Input.ICommand.
/// </summary>
public interface ISyncCommand : ICommand
{
    /// <summary>
    /// Triggers the <see cref="CanExecuteChanged"/> event.
    /// </summary>
    void FireCanExecuteChanged();
}