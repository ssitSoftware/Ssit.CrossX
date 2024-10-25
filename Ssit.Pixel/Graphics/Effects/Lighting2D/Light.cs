using System.Numerics;

namespace Ssit.Pixel.Graphics.Effects.Lighting2D;

/// <summary>
/// Represents a light source in a 2D environment.
/// </summary>
public class Light
{
    /// <summary>
    /// Gets or sets the position of the light source in a 2D environment.
    /// </summary>
    /// <value>
    /// A <see cref="Vector2"/> representing the coordinates of the light source in 2D space.
    /// </value>
    public Vector2 Position { get; set; }

    /// <summary>
    /// Gets or sets the depth of the light source in a 2D environment.
    /// </summary>
    /// <value>
    /// A float representing the depth position of the light source.
    /// </value>
    public float Depth { get; set; }

    /// <summary>
    /// Gets or sets the range within which the light source affects the 2D environment.
    /// </summary>
    /// <value>
    /// A <see cref="float"/> representing the radius of influence of the light source.
    /// </value>
    public float Range { get; set; }

    /// <summary>
    /// Gets or sets the color of the light source in a 2D environment.
    /// </summary>
    /// <value>
    /// A <see cref="RgbaColor"/> representing the color of the light source.
    /// </value>
    public RgbaColor Color { get; set; }
}