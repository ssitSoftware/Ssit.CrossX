namespace Ssit.CrossX.Input;

/// <summary>
/// Interface representing the mapping of input controls.
/// </summary>
public interface IInputMapping
{
    /// <summary>
    /// Retrieves the value of the specified axis for the mapped input controls.
    /// </summary>
    /// <param name="axis">The name of the axis to retrieve the value for.</param>
    /// <returns>The value of the axis, ranging from -1 to 1.</returns>
    float GetAxis(string axis);

    /// <summary>
    /// Gets the current state of the specified button.
    /// </summary>
    /// <param name="button">The name of the button for which to get the state.</param>
    /// <returns>A <see cref="ButtonState"/> representing the current state of the button.</returns>
    ButtonState GetButton(string button);
}