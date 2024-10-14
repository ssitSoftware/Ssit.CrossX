namespace Ssit.Pixel.Framework.Audio;

/// <summary>
/// Defines the interface for managing sound within the framework.
/// </summary>
public interface ISoundManager
{
    /// <summary>
    /// Gets or sets the primary sound listener.
    /// This represents the listener object with its position, velocity and direction.
    /// </summary>
    SoundListener Listener { get; set; }

    /// <summary>
    /// Gets or sets the master volume level for the sound.
    /// </summary>
    /// <remarks>
    /// The master volume is a value between 0.0 and 1.0, where 0.0 means no sound and 1.0 means full volume.
    /// Changes to this property will affect the overall volume of all sounds in game/app.
    /// </remarks>
    float MasterVolume { get; set; }
}