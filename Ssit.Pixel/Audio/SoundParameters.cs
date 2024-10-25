namespace Ssit.Pixel.Audio;

/// <summary>
/// SoundParameters struct holds various properties and parameters related to sound effects.
/// It is used in conjunction with sound emitting structures and interfaces to provide detailed control over sound playback characteristics.
/// </summary>
public struct SoundParameters
{
    /// <summary>
    /// Represents the parameters related to sound configuration.
    /// </summary>
    public SoundParameters()
    {
    }

    /// <summary>
    /// Gets or sets the volume level for the sound.
    /// The volume is a float value where 0 represents silence and 1 represents the maximum volume.
    /// Default value is 1.
    /// </summary>
    private float Volume { get; set; } = 1;

    /// <summary>
    /// Gets or sets the panning value of the sound.
    /// The panning value ranges from -1.0 (full left) to 1.0 (full right),
    /// with 0.0 representing the center (default).
    /// </summary>
    private float Pan { get; set; } = 0;

    /// <summary>
    /// Gets or sets the pitch of the sound.
    /// The pitch determines how high or low the sound is perceived.
    /// A value of 1.0 represents the original pitch, values greater than 1.0 increase the pitch, and values less than 1.0 decrease the pitch.
    /// </summary>
    private float Pitch { get; set; } = 1;
}