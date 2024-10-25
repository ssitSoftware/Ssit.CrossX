using System;

namespace Ssit.Pixel.Graphics;

public interface IEffect: IDisposable
{
     /// <summary>
     /// Gets or sets a value indicating whether existing pixels should be overwritten.
     /// </summary>
     /// <remarks>
     /// If set to true, the effect will overwrite pixels that are already present.
     /// If set to false, the effect will be applied only to empty or transparent pixels.
     /// This reduces the number of per pixel calculations, when set to false and rendering is performed int front to back order.
     /// </remarks>
     bool OverwriteExistingPixels { get; set; }
}