using System;

namespace Ssit.Pixel.Graphics;

/// <summary>
/// Represents a texture in the graphics framework.
/// </summary>
public interface ITexture: IDisposable
{
    /// <summary>
    /// Gets the size of the texture as a <see cref="Ssit.Pixel.Size"/> structure.
    /// </summary>
    Size Size { get; }
    TextureMaps TextureMaps { get; }
}