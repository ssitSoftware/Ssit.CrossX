namespace Ssit.CrossX.Graphics.Effects;

public interface IBloomEffect: IEffect
{
    /// <summary>
    /// Luminance level above which pixels start contributing to the bloom. Range: 0.0 (everything blooms) to 1.0 (only pure white blooms).
    /// </summary>
    float BloomThreshold { get; set; }

    /// <summary>
    /// Multiplier applied to the bloom contribution before it's added back to the base color. Range: 0.0 (no bloom) upwards; typical values are 0.0 to ~2.0, values much above that quickly blow out to white.
    /// </summary>
    float BloomIntensity { get; set; }
}