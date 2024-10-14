using System;

namespace Ssit.Pixel.Framework.Audio;

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
    /// Plays the sound effect once with the specified volume.
    /// </summary>
    /// <param name="volume">The volume at which the sound should be played. The default value is 1.0f.</param>
    void PlayOnce(float volume = 1.0f);
}