using System.Numerics;

namespace Ssit.Pixel.Framework.Audio;

/// <summary>
/// The SoundEmitter struct is used to represent the position and velocity of a sound source in 3D space.
/// It is primarily used in conjunction with sound effect instances to provide spatial audio effects.
/// </summary>
public struct SoundEmitter
{
    /// Represents a sound-emitting source in a 3D space.
    public SoundEmitter()
    {
    }

    /// <summary>
    /// Gets or sets the position of the sound emitter in 3D space.
    /// </summary>
    public Vector3 Position { get; set; } = Vector3.Zero;

    /// <summary>
    /// Gets or sets the velocity of the sound emitter.
    /// The velocity determines the speed and direction at which the emitter is moving.
    /// </summary>
    public Vector3 Velocity { get; set; } = Vector3.One;
}