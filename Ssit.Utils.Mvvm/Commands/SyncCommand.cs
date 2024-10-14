using System;

namespace Ssit.Utils.Mvvm.Commands;

/// <summary>
/// Represents a synchronous command that implements the ISyncCommand interface.
/// </summary>
public class SyncCommand: ISyncCommand
{
    private readonly Action<object> _executeAction;
    private readonly Func<object, bool> _canExecuteFunction;

    public event EventHandler CanExecuteChanged;

    public SyncCommand(Action<object> action) => _executeAction = action;

    public SyncCommand(Action<object> action, Func<object, bool> canExecute)
    {
        _executeAction = action ?? throw new ArgumentNullException(nameof(action));
        _canExecuteFunction = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
    }

    public SyncCommand(Action action)
    {
        if (action is null)
            throw new ArgumentNullException(nameof(action));
            
        _executeAction = o => action.Invoke();
    }

    public SyncCommand(Action action, Func<bool> canExecute)
    {
        if (action is null)
            throw new ArgumentNullException(nameof(action));
            
        if (canExecute is null)
            throw new ArgumentNullException(nameof(canExecute));
            
        _executeAction = o => action.Invoke();
        _canExecuteFunction = o => canExecute.Invoke();
    }

    /// <summary>
    /// Raises the CanExecuteChanged event, indicating that the ability of the command to execute has changed.
    /// </summary>
    public void FireCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

    /// <summary>
    /// Determines whether the command can execute in its current state.
    /// </summary>
    /// <param name="parameter">Data used by the command. If the command does not require any data to be passed, this parameter can be set to null.</param>
    /// <returns>True if the command can execute; otherwise, false.</returns>
    public bool CanExecute(object parameter) => _canExecuteFunction?.Invoke(parameter) ?? true;

    /// <summary>
    /// Executes the command with the specified parameter.
    /// </summary>
    /// <param name="parameter">Data used by the command. If the command does not require any data to be passed, this parameter can be set to null.</param>
    public void Execute(object parameter)
    {
        if (!CanExecute(parameter))
            return;
        _executeAction?.Invoke(parameter);
    }
}

/// <summary>
/// Represents a synchronous command implementation for the ISyncCommand interface with specified parameter type.
/// </summary>
public class SyncCommand<TParameter> : SyncCommand
{
    public SyncCommand(Action<TParameter> action)
        : base(
            action.ToGeneralCommandDelegate())
    {
    }
        
    public SyncCommand(
        Action<TParameter> action, 
        Func<TParameter, bool> canExecute)
        : base(
            action.ToGeneralCommandDelegate(),
            canExecute.ToGeneralCommandDelegate())
    {
    }
}