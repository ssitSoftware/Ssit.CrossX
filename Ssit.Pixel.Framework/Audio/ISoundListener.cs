using System.Numerics;

namespace Ssit.Pixel.Framework.Audio;

/// <summary>
/// Represents a sound listener within the audio framework.
/// This struct is used to manage and interact with sound sources in 3D space.
/// </summary>
public interface ISoundListener
{
    /// <summary>
    /// Gets or sets the position of the sound listener in 3D space.
    /// </summary>
    Vector3 Position { get; }

    /// <summary>
    /// Represents the velocity of the sound listener in 3D space.
    /// </summary>
    Vector3 Velocity { get; }

    /// <summary>
    /// Gets or sets the direction the listener is facing.
    /// The direction is represented as a Vector3.
    /// </summary>
    Vector3 Direction { get; }
}