using System;

namespace Ssit.Pixel.Audio;

/// <summary>
/// Defines the interface for a sound effect. Provides methods to create
/// instances of the sound effect and play the sound effect.
/// </summary>
public interface ISoundEffect: IDisposable
{
    /// <summary>
    /// Creates a new instance of the sound effect.
    /// </summary>
    /// <returns>
    /// New sound effect instance object.
    /// </returns>
    ISoundEffectInstance CreateInstance();

    /// <summary>
    /// Plays the sound effect once with the specified volume, pan, and pitch values.
    /// </summary>
    /// <param name="volume">The volume level of the sound effect, ranging from 0.0 (silent) to 1.0 (full volume).</param>
    /// <param name="pan">The stereo pan position of the sound effect, ranging from -1.0 (left) to 1.0 (right).</param>
    /// <param name="pitch">The pitch level of the sound effect, where 1.0 is the normal pitch.</param>
    void PlayOnce(float volume = 1.0f, float pan = 0.0f, float pitch = 1.0f);
}