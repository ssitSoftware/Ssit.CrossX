using System.IO;
using SkiaSharp;

namespace Ssit.Pixel.Framework.Utils;

/// <summary>
/// Provides functionality to load images and extract pixel data.
/// </summary>
public static class ImagesLoader
{
    /// <summary>
    /// Loads an image from a stream and extracts its pixel data into an array.
    /// </summary>
    /// <param name="stream">The stream containing the image data.</param>
    /// <returns>A 2-dimensional array of RgbaColor representing the pixel data of the image.</returns>
    public static RgbaColor[,] LoadImage(Stream stream)
    {
        using var img = SKBitmap.Decode(stream);
        var pixels = img.Pixels;

        var w = img.Width;
        var h = img.Height;
        
        var data = new RgbaColor[w, h];
        
        for (var x = 0; x < w; ++x)
        {
            for (var y = 0; y < h; ++y)
            {
                var index = x + y * w;

                var col = pixels[index];
                data[x, y] = new RgbaColor(col.Red, col.Green, col.Blue, col.Alpha);
            }
        }

        return data;
    }
}