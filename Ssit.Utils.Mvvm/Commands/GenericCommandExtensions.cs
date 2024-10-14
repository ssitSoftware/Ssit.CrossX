using System;
using System.Threading.Tasks;

namespace Ssit.Utils.Mvvm.Commands;

internal static class GenericCommandExtensions
{
    /// <summary>
    /// Converts parametrized delegate to general (object) parameter one with proper behavior for value and reference types.
    /// </summary>
    /// <param name="action">Delegate to call when command is executed.</param>
    /// <typeparam name="TParameter">Type of parameter passed to command.</typeparam>
    /// <returns>General type delegate.</returns>
    /// <exception cref="ArgumentNullException">action cannot be null.</exception>
    public static Func<object, Task> ToGeneralCommandDelegate<TParameter>(this Func<TParameter, Task> action)
    {
        if (action is null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        return o =>
        {
            // If parameter is of desired type, just call delegate.
            if (o is TParameter parameter)
            {
                return action(parameter);
            }

            // If parameter is not of desired type and the type of parameter is reference type, call delegate with null.
            if (!typeof(TParameter).IsValueType)
            {
                return action((TParameter)(object)null);
            }

            // If it is impossible to call canExecute, do nothing and just return completed task.
            return Task.CompletedTask;
        };
    }

    /// <summary>
    /// Converts parametrized delegate to general (object) parameter one with proper behavior for value and reference types.
    /// </summary>
    /// <param name="action">Delegate to call when command is executed.</param>
    /// <typeparam name="TParameter">Type of parameter passed to command.</typeparam>
    /// <returns>General type delegate.</returns>
    /// <exception cref="ArgumentNullException">action cannot be null.</exception>
    public static Action<object> ToGeneralCommandDelegate<TParameter>(this Action<TParameter> action)
    {
        if (action is null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        return o =>
        {
            // If parameter is of desired type, just call delegate.
            if (o is TParameter parameter)
            {
                action(parameter);
                return;
            }

            // If parameter is not of desired type and the type of parameter is reference type, call delegate with null.
            else if (!typeof(TParameter).IsValueType)
            {
                action((TParameter)(object)null);
            }
        };
    }

    /// <summary>
    /// Converts parametrized delegate to general (object) parameter one with proper behavior for value and reference types.
    /// </summary>
    /// <param name="canExecute">Delegate returning if command can be executed.</param>
    /// <typeparam name="TParameter">Type of parameter passed to command.</typeparam>
    /// <returns>General type delegate.</returns>
    /// <exception cref="ArgumentNullException">canExecute cannot be null</exception>
    public static Func<object, bool> ToGeneralCommandDelegate<TParameter>(this Func<TParameter, bool> canExecute)
    {
        if (canExecute is null)
        {
            throw new ArgumentNullException(nameof(canExecute));
        }

        return o =>
        {
            // If parameter is of desired type, just call delegate.
            if ((o is TParameter parameter))
            {
                return canExecute(parameter);
            }

            // If parameter is not of desired type and the type of parameter is reference type, call delegate with null.
            if (!typeof(TParameter).IsValueType)
            {
                return canExecute((TParameter)(object)null);
            }

            // If it is impossible to call canExecute, disable command.
            return false;
        };
    }
}