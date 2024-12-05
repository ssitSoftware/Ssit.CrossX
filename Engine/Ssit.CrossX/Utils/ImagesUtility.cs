using System;
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

    public static Stream GetStream(RgbaColor[,] colors)
    {
        using var bitmap = new SKBitmap(colors.GetLength(0), colors.GetLength(1));

        var bytes = new byte[colors.GetLength(0) * colors.GetLength(1) * 4];
        
        var stride = bitmap.Width * 4;

        var w = bitmap.Width;
        var h = bitmap.Height;
        
        for (var x = 0; x < w; ++x)
        {
            for (var y = 0; y < h; ++y)
            {
                var col = colors[x, y];
                bytes[0 + x * 4 + y * stride] = col.R;
                bytes[1 + x * 4 + y * stride] = col.G;
                bytes[2 + x * 4 + y * stride] = col.B;
                bytes[3 + x * 4 + y * stride] = col.A; 
            }
        }

        unsafe
        {
            fixed (byte* pBytes = bytes)
            {
                bitmap.SetPixels((IntPtr)pBytes);
            }
        }

        var memoryStream = new MemoryStream();
        bitmap.Encode(memoryStream, SKEncodedImageFormat.Png, 100);
        memoryStream.Seek(0, SeekOrigin.Begin);
        return memoryStream;
    }
}