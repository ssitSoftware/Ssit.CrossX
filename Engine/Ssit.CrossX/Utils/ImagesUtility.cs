using System.IO;
using SkiaSharp;

namespace Ssit.CrossX.Utils;

/// <summary>
/// Provides functionality to load images and extract pixel data.
/// </summary>
public static class ImagesUtility
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

    public static Stream GetStream(RgbaColor[,] color)
    {
        using var bitmap = new SKBitmap(color.GetLength(0), color.GetLength(1));
        
        var w = bitmap.Width;
        var h = bitmap.Height;
        
        for (var x = 0; x < w; ++x)
        {
            for (var y = 0; y < h; ++y)
            {
                var col = color[x, y];
                bitmap.SetPixel(x, y, new SKColor(col.R, col.G, col.B, col.A));
            }
        }

        var memoryStream = new MemoryStream();
        bitmap.Encode(memoryStream, SKEncodedImageFormat.Png, 100);
        memoryStream.Seek(0, SeekOrigin.Begin);
        return memoryStream;
    }
}