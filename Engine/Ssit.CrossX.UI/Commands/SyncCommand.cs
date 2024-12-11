using System;
using System.Windows.Input;

namespace Ssit.CrossX.Commands;

public class SyncCommand: ICommand
{
    private readonly Func<object, bool> _canExecute;
    private readonly Action<object> _execute;
    
    public event EventHandler CanExecuteChanged;
    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

    public SyncCommand(Action<object> execute, Func<object, bool> canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }
    
    public SyncCommand(Action execute, Func<bool> canExecute = null)
    {
        _execute = execute != null ? o => execute() : throw new ArgumentNullException(nameof(execute));
        _canExecute = o => canExecute?.Invoke() ?? true;
    }
    
    public bool CanExecute(object parameter)
    {
        return _canExecute?.Invoke(parameter) ?? true;
    }

    public void Execute(object parameter)
    {
        if (CanExecute(parameter))
        {
            _execute?.Invoke(parameter);
        }
    }
}