using System;

namespace Ssit.Pixel.Framework.Audio;

/// <summary>
/// Represents an instance of a sound effect, supporting operations like play, stop, pause, and resume.
/// </summary>
public interface ISoundEffectInstance: IDisposable
{
    /// <summary>
    /// Gets or sets the SoundEmitter associated with a sound effect instance.
    /// The Emitter controls the position and velocity of the sound in a 3D space,
    /// allowing for spatial audio effects.
    /// </summary>
    SoundEmitter Emitter { get; set; }

    /// <summary>
    /// Gets or sets the sound parameters associated with the sound effect instance.
    /// </summary>
    /// <remarks>
    /// These parameters define various properties of the sound effect such as volume, pitch, etc.
    /// </remarks>
    SoundParameters Parameters { get; set; }

    /// <summary>
    /// Begins playback of the sound effect instance.
    /// </summary>
    void Play();

    /// <summary>
    /// Stops the playback of the sound effect instance that is currently playing.
    /// </summary>
    void Stop();

    /// <summary>
    /// Pauses the sound effect instance.
    /// </summary>
    void Pause();

    /// <summary>
    /// Resumes a paused sound effect instance.
    /// </summary>
    void Resume();
}