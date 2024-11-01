using System;

namespace Ssit.CrossX.Audio;

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
    /// Plays the sound effect once with the specified volume, pitch, and optional spatial emitter.
    /// </summary>
    /// <param name="volume">The volume level of the sound effect, ranging from 0.0 (silent) to 1.0 (full volume).</param>
    /// <param name="pitch">The pitch level of the sound effect, where 1.0 is the normal pitch.</param>
    /// <param name="emitter">An optional sound emitter for spatial audio effects. If set to null, the sound plays without spatial positioning.</param>
    void PlayOnce(float volume = 1.0f, float pitch = 1.0f, ISoundEmitter emitter = null);
}