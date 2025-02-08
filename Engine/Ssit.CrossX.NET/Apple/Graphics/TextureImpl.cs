#if __IOS__ || __MACCATALYST__

using System.IO;
using CoreGraphics;
using Foundation;
using Metal;
using MetalKit;
using MetalPerformanceShaders;
using SkiaSharp;
using Ssit.CrossX.Graphics;

namespace Ssit.CrossX.NET.Apple.Graphics;

public class TextureImpl: ITexture
{
    private IMTLTexture _diffuseTexture;
    private IMTLTexture _normalTexture;
    private IMTLTexture _glowTexture;
    
    public TextureImpl(MTKTextureLoader textureLoader, LoadTextureParameters loadTextureParameters)
    {
        _diffuseTexture = LoadTexture(textureLoader, loadTextureParameters.DiffuseMapStream);
        _normalTexture = LoadTexture(textureLoader, loadTextureParameters.NormalMapStream);
        _glowTexture = LoadTexture(textureLoader, loadTextureParameters.GlowMapStream);
        
        TextureMaps = TextureMaps.None;

        if (_diffuseTexture != null)
        {
            TextureMaps |= TextureMaps.Diffuse;
            Size = new Size((int) _diffuseTexture.Width, (int) _diffuseTexture.Height);
        }
        
        if (_normalTexture != null)
        {
            TextureMaps |= TextureMaps.NormalMap;

            if (Size.Width == 0 || Size.Height == 0)
            {
                Size = new Size((int) _normalTexture.Width, (int) _normalTexture.Height);
            }
        }
        
        if (_glowTexture != null)
        {
            TextureMaps |= TextureMaps.GlowMap;

            if (Size.Width == 0 || Size.Height == 0)
            {
                Size = new Size((int) _glowTexture.Width, (int) _glowTexture.Height);
            }
        }
    }

    private IMTLTexture LoadTexture(MTKTextureLoader textureLoader, Stream stream)
    {
        if (stream is null)
        {
            return null;
        }

        using var bmp = SKBitmap.Decode(stream);
        var colors = bmp.Pixels;

        for (var idx = 0; idx < colors.Length; idx++)
        {
            var af = colors[idx].Alpha / 255f;

            var r = (byte)(colors[idx].Red * af);
            var g = (byte)(colors[idx].Green * af);
            var b = (byte)(colors[idx].Blue * af);
            
            colors[idx] = new SKColor(r, g, b, colors[idx].Alpha);
        }

        bmp.Pixels = colors;
        
        using var memoryStream = new MemoryStream();
        bmp.Encode(memoryStream, SKEncodedImageFormat.Png, 100);
        
        memoryStream.Seek(0, SeekOrigin.Begin);
        using var data = NSData.FromStream(memoryStream);
        
        if (data is null)
        {
            return null;
        }
        
        var texture = textureLoader.FromData(data!, new MTKTextureLoaderOptions
        {
            Srgb = false
        }, out var error);
        return texture;
    }

    public void Dispose()
    {
        _diffuseTexture?.Dispose();
        _diffuseTexture = null;
        
        _normalTexture?.Dispose();
        _normalTexture = null;
        
        _glowTexture?.Dispose();
        _glowTexture = null;
    }

    public Size Size { get; }
    public TextureMaps TextureMaps { get; }
    
    public TTextureMap GetMap<TTextureMap>(TextureMaps map) where TTextureMap : class
    {
        switch (map)
        {
            case TextureMaps.Diffuse:
                return _diffuseTexture as TTextureMap;
            
            case TextureMaps.NormalMap:
                return _normalTexture as TTextureMap;
            
            case TextureMaps.GlowMap:
                return _glowTexture as TTextureMap;
        }

        return null;
    }
}

#endif