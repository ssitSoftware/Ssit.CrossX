namespace Ssit.Pixel.Input;

/// <summary>
/// Service for game controller input handling.
/// </summary>
public interface IGameControllers
{
    /// <summary>
    /// Gets or sets the vibration force for the game controllers.
    /// </summary>
    /// <remarks>
    /// The vibration force is represented as a byte value.
    /// </remarks>
    byte VibrationForce { get; set; }

    /// <summary>
    /// Gets the state of a specified game controller button for a given player.
    /// </summary>
    /// <param name="player">The player index (e.g., 0 for the first player).</param>
    /// <param name="button">The game controller button whose state is to be retrieved.</param>
    /// <returns>The state of the specified game controller button.</returns>
    ButtonState GetButton(int player, GameControllerButton button);

    /// <summary>
    /// Retrieves the value of the specified axis for the given player.
    /// </summary>
    /// <param name="player">The player index to get the axis value for.</param>
    /// <param name="axis">The specific axis to retrieve the value from.</param>
    /// <returns>A float representing the axis value, ranging from -1.0 to 1.0.</returns>
    float GetAxis(int player, GameControllerAxis axis);
}