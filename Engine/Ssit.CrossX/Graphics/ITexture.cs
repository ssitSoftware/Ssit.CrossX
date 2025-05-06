using System;

namespace Ssit.CrossX.Graphics;

/// <summary>
/// Represents a texture in the graphics framework.
/// </summary>
public interface ITexture: IDisposable
{
    /// <summary>
    /// Gets the size of the texture as a <see cref="Ssit.CrossX.Size"/> structure.
    /// </summary>
    Size Size { get; }
    TextureMaps TextureMaps { get; }
    TTextureMap GetMap<TTextureMap>(TextureMaps map) where TTextureMap : class;
}