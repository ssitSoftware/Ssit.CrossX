using System;
using System.Threading.Tasks;

namespace Ssit.Utils.Mvvm.Commands;

/// <summary>
/// Represents an asynchronous command that can be used in MVVM patterns.
/// </summary>
public class AsyncCommand : IAsyncCommand
{
    private readonly Func<object, Task> _executeAction;
    private readonly Func<object, bool> _canExecuteFunction;

    private readonly Action<Exception> _onException;

    public event EventHandler CanExecuteChanged;

    private volatile int _isExecuting;

    public AsyncCommand(Func<object, Task> action, Action<Exception> onException = null)
    {
        _executeAction = action ?? throw new ArgumentNullException(nameof(action));
        _onException = onException;
    }

    public AsyncCommand(
        Func<object, Task> action,
        Func<object, bool> canExecute,
        Action<Exception> onException = null)
    {
        _executeAction = action ?? throw new ArgumentNullException(nameof(action));
        _canExecuteFunction = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        _onException = onException;
    }

    public AsyncCommand(Func<Task> action, Action<Exception> onException = null)
    {
        if (action is null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        _executeAction = o => action.Invoke();
        _onException = onException;
    }

    public AsyncCommand(
        Func<Task> action,
        Func<bool> canExecute,
        Action<Exception> onException = null)
    {
        if (action is null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        if (canExecute is null)
        {
            throw new ArgumentNullException(nameof(canExecute));
        }

        _executeAction = o => action.Invoke();
        _canExecuteFunction = o => canExecute.Invoke();
        _onException = onException;
    }

    public AsyncCommand(
        Func<object, Task> action,
        Func<bool> canExecute,
        Action<Exception> onException = null)
    {
        if (canExecute is null)
        {
            throw new ArgumentNullException(nameof(canExecute));
        }

        _executeAction = action ?? throw new ArgumentNullException(nameof(action));
        _canExecuteFunction = o => canExecute.Invoke();
        _onException = onException;
    }

    /// <summary>
    /// Triggers the <see cref="CanExecuteChanged"/> event.
    /// </summary>
    public void FireCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Determines whether the command can execute in its current state.
    /// </summary>
    /// <param name="parameter">The parameter to pass to the command's action.</param>
    /// <returns>True if the command can execute; otherwise, false.</returns>
    public bool CanExecute(object parameter)
    {
        return _isExecuting == 0 && (_canExecuteFunction?.Invoke(parameter) ?? true);
    }

    /// <summary>
    /// Executes the command asynchronously with the provided parameter.
    /// </summary>
    /// <param name="parameter">The parameter to pass to the command's action.</param>
    public async void Execute(object parameter)
    {
        if (!CanExecute(parameter))
        {
            return;
        }

        try
        {
            _isExecuting = 1;
            FireCanExecuteChanged();
            await Task.Delay(1);
            await _executeAction.Invoke(parameter);
        }
        catch (AggregateException exception)
        {
            _onException?.Invoke(exception);
        }
        finally
        {
            _isExecuting = 0;
            FireCanExecuteChanged();
        }
    }

    /// <summary>
    /// Executes the command asynchronously with the given parameter.
    /// </summary>
    /// <param name="parameter">The parameter to pass to the command's action.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task ExecuteAsync(object parameter) => _executeAction.Invoke(parameter);
}

/// <summary>
/// Represents an asynchronous command that can be used in MVVM patterns with specified parameter type.
/// </summary>
public class AsyncCommand<TParameter> : AsyncCommand
{
    public AsyncCommand(Func<TParameter, Task> action, Action<Exception> onException = null)
        : base(
            action.ToGeneralCommandDelegate(),
            onException)
    {
    }
        
    public AsyncCommand(
        Func<TParameter, Task> action, 
        Func<TParameter, bool> canExecute, 
        Action<Exception> onException = null)
        : base(
            action.ToGeneralCommandDelegate(),
            canExecute.ToGeneralCommandDelegate(),
            onException)
    {
    }
}