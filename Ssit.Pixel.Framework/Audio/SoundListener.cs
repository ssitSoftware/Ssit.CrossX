using System.Numerics;

namespace Ssit.Pixel.Framework.Audio;

/// <summary>
/// Represents a sound listener within the audio framework.
/// This struct is used to manage and interact with sound sources in 3D space.
/// </summary>
public struct SoundListener
{
    /// <summary>
    /// Represents a sound listener in the audio framework.
    /// </summary>
    public SoundListener()
    {
    }

    /// <summary>
    /// Gets or sets the position of the sound listener in 3D space.
    /// The default position is initialized to (0, 0, 10).
    /// </summary>
    private Vector3 Position { get; set; } = new (0, 0, 10);

    /// <summary>
    /// Represents the velocity of the sound listener in 3D space.
    /// </summary>
    private Vector3 Velocity { get; set; } = Vector3.Zero;

    /// <summary>
    /// Gets or sets the direction the listener is facing.
    /// The direction is represented as a Vector3 where the default value is (0, 0, -1), indicating forward.
    /// </summary>
    private Vector3 Direction { get; set; } = new (0, 0, -1);
}