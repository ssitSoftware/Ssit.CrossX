using System.Numerics;

namespace Ssit.Pixel.Graphics.Effects.Lighting2D;

public class SpotLight : Light
{
    /// <summary>
    /// Gets or sets the direction of the light source in 2D space.
    /// </summary>
    /// <value>
    /// A <see cref="Vector2"/> representing the direction vector of the light source.
    /// </value>
    public Vector2 Direction { get; set; }

    /// <summary>
    /// Gets or sets the angle of the light source in radians.
    /// </summary>
    /// <value>
    /// A <see cref="float"/> representing the angle of the light source.
    /// </value>
    public float Angle { get; set; }
}