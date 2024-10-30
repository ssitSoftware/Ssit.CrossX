#if __IOS__ || __MACCATALYST__

using System.IO;
using Foundation;
using Metal;
using MetalKit;
using Ssit.Pixel.Graphics;

namespace Ssit.Pixel.NET.Apple.Graphics;

public class TextureImpl: ITexture
{
    private IMTLTexture _diffuseTexture;
    private IMTLTexture _normalTexture;

    public TextureImpl(MTKTextureLoader textureLoader, LoadTextureParameters loadTextureParameters)
    {
        _diffuseTexture = LoadTexture(textureLoader, loadTextureParameters.DiffuseMapStream);
        _normalTexture = LoadTexture(textureLoader, loadTextureParameters.NormalMapStream);

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
    }

    private IMTLTexture LoadTexture(MTKTextureLoader textureLoader, Stream stream)
    {
        if (stream is null)
        {
            return null;
        }
        
        using var data = NSData.FromStream(stream);
        
        if (data is null)
        {
            return null;
        }
        
        return textureLoader.FromData(data, new MTKTextureLoaderOptions(), out var error);
    }

    public void Dispose()
    {
        _diffuseTexture?.Dispose();
        _diffuseTexture = null;
        
        _normalTexture?.Dispose();
        _normalTexture = null;
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
        }

        return null;
    }
}

#endif