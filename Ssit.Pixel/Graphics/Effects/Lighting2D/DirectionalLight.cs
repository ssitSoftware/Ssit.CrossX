using System.Numerics;

namespace Ssit.Pixel.Graphics.Effects.Lighting2D;

public class DirectionalLight
{
    /// <summary>
    /// Gets or sets the direction of the light source in 2D space.
    /// </summary>
    /// <value>
    /// A <see cref="Vector2"/> representing the direction vector of the light source.
    /// </value>
    public Vector2 Direction { get; set; }
    
    /// <summary>
    /// Gets or sets the color of the light source in a 2D environment.
    /// </summary>
    /// <value>
    /// A <see cref="RgbaColor"/> representing the color of the light source.
    /// </value>
    public RgbaColor Color { get; set; }
}