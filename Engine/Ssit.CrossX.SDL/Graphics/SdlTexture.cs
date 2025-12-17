using SDL;
using SkiaSharp;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.SDL.Common;
using Ssit.CrossX.Utils;
using static SDL.SDL3_image;
using static SDL.SDL3; 

namespace Ssit.CrossX.SDL.Graphics;

// ReSharper disable once ClassNeverInstantiated.Global
public unsafe class SdlTexture: ITexture
{
    private readonly SdlHandles _handles;
    private readonly ISdlPalette _sdlPalette;
    private bool _disposed;
    
    private SdlHandle<SDL_Texture> _textureDiff;
    private SdlHandle<SDL_Texture> _textureGlow;

    private readonly SdlHandle<SDL_Surface> _surfaceDiff;
    private readonly SdlHandle<SDL_Surface> _surfaceGlow;

    public Size Size { get; }

    public TextureMaps TextureMaps { get; }

    private readonly bool _useDiffuseAsGlow;

    public SdlTexture(SdlHandles handles, LoadTextureParameters parameters, ISdlPalette sdlPalette = null)
    {
        _handles = handles;
        _sdlPalette = sdlPalette;
        bool updatePalette = false;
        
        if (parameters.DiffuseMapStream is not null)
        {
            int width, height;

            if (sdlPalette == null || parameters.ColorMode != LoadTextureColorMode.Default)
            {
                (_textureDiff, width, height) =
                    LoadTexture(parameters.DiffuseMapStream, handles.Renderer, parameters.ColorMode);
            }
            else
            {
                (_textureDiff, _surfaceDiff) = LoadIndexedImage(parameters.DiffuseMapStream, sdlPalette.OriginalPalette);
                
                width = _surfaceDiff.Pointer->w;
                height = _surfaceDiff.Pointer->h;

                updatePalette = true;
            }

            Size = new Size(width, height);
            TextureMaps = TextureMaps.Diffuse;
        }
        
        if (parameters.GlowMapStream is not null)
        {
            int width, height;
            if (sdlPalette == null || parameters.ColorMode is LoadTextureColorMode.NoPalette)
            {
                (_textureGlow, width, height) =
                    LoadTexture(parameters.GlowMapStream, handles.Renderer, parameters.ColorMode);
            }
            else
            {
                (_textureGlow, _surfaceGlow) = LoadIndexedImage(parameters.GlowMapStream, sdlPalette.OriginalPalette);
                
                width = _surfaceGlow.Pointer->w;
                height = _surfaceGlow.Pointer->h;

                updatePalette = true;
            }

            if (Size == Size.Zero)
            {
                Size = new Size(width, height);
            }
            else
            {
                if (Size != new Size(width, height)) 
                    throw new InvalidDataException("Glow texture size is not equal to diffuse texture size");
            }

            TextureMaps |= TextureMaps.GlowMap;
        }

        if (parameters.ColorMode == LoadTextureColorMode.DiffuseAndGlow && _surfaceGlow == null)
        {
            _useDiffuseAsGlow = true;
            TextureMaps |= TextureMaps.GlowMap;
        }
        
        if (_sdlPalette != null && parameters.ColorMode == LoadTextureColorMode.Default)
        {
            _sdlPalette.OnPaletteChanged += UpdateSdlPalette;
        }

        if (updatePalette)
        {
            UpdateSdlPalette();
        }
    }

    private (SdlHandle<SDL_Texture>, SdlHandle<SDL_Surface>) LoadIndexedImage(Stream stream, IReadOnlyList<RgbaColor> palette)
    {
        var dict = new Dictionary<SKColor, byte>();

        using var bmp = SKBitmap.Decode(stream);

        var pixels = bmp.Pixels;
        
        SDL_Surface* surfacePtr = SDL_CreateSurface(bmp.Width, bmp.Height, SDL_PixelFormat.SDL_PIXELFORMAT_INDEX8);
        
        var stride = surfacePtr->pitch;
        var indices = new byte[stride * bmp.Height];

        for (var y = 0; y < bmp.Height; y++)
        {
            for(var x = 0; x < bmp.Width; x++)
            {
                var color = pixels[y * bmp.Width + x];
                
                if (!dict.TryGetValue(color, out var index))
                {
                    var dist = color.Alpha < 224 ? 0 : float.MaxValue;
                    
                    for (var idx = 1; idx < palette.Count && dist > 0; ++idx)
                    {
                        var d = color.DistanceTo(palette[idx]);
                        if (d < dist)
                        {
                            dist = d;
                            index = (byte)idx;
                        }
                    }

                    dict[color] = index;
                }
                indices[y*stride+x] = index;
            }
        }

        fixed (byte* ptr = indices)
        {
            byte* surfPixels = (byte*)surfacePtr->pixels;
            Buffer.MemoryCopy(ptr, surfPixels, indices.Length, indices.Length);
        }
        
        var texturePtr = SDL_CreateTexture(_handles.Renderer, SDL_PixelFormat.SDL_PIXELFORMAT_ABGR8888, SDL_TextureAccess.SDL_TEXTUREACCESS_STATIC, surfacePtr->w, surfacePtr->h);
        return (new SdlHandle<SDL_Texture>(texturePtr), new SdlHandle<SDL_Surface>(surfacePtr));
    }

    private void UpdateSdlPalette(ref SdlHandle<SDL_Texture> texturePtr, SdlHandle<SDL_Surface> surfacePtr, SdlHandle<SDL_Palette> palettePtr)
    {
        if (surfacePtr is null || surfacePtr.Pointer is null)
        {
            return;
        }

        SDL_SetSurfacePalette(surfacePtr.Pointer, palettePtr.Pointer);

        var newSurface = SDL_ConvertSurface(surfacePtr.Pointer, SDL_PixelFormat.SDL_PIXELFORMAT_ABGR8888);
        SDL_PremultiplySurfaceAlpha(newSurface, true);

        if (texturePtr != null && texturePtr.Pointer != null)
        {
            SDL_DestroyTexture(texturePtr.Pointer);
        }
        
        var texture = SDL_CreateTextureFromSurface(_handles.Renderer, newSurface);
        texturePtr = new SdlHandle<SDL_Texture>(texture);

        SDL_DestroySurface(newSurface);
    }
    
    private void UpdateSdlPalette()
    {
        UpdateSdlPalette(ref _textureDiff, _surfaceDiff, _sdlPalette.PaletteHandle);

        if (_surfaceGlow != null && _surfaceGlow.Pointer != null)
        {
            UpdateSdlPalette(ref _textureGlow, _surfaceGlow, _sdlPalette.GlowPaletteHandle);
        }
        else if (_textureGlow != null && _textureGlow.Pointer != null)
        {
            SDL_DestroyTexture(_textureGlow.Pointer);
            _textureGlow = null;
        }
    }

    private (SdlHandle<SDL_Texture> ptr, int w, int h) LoadTexture(Stream stream, SDL_Renderer* renderer, LoadTextureColorMode colorMode)
    {
        var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        
        SDL_Surface* surface;
        
        var bytes = memoryStream.GetBuffer();
        
        fixed (byte* ptr = bytes)
        {
            nint ptrN = (nint)ptr;
            var io = SDL_IOFromMem(ptrN, (nuint)bytes.Length);
            surface = IMG_Load_IO(io, true);
        }

        if (surface->format != SDL_PixelFormat.SDL_PIXELFORMAT_ABGR8888)
        {
            var newSurface = SDL_ConvertSurface(surface, SDL_PixelFormat.SDL_PIXELFORMAT_ABGR8888);
            SDL_DestroySurface(surface);
            surface = newSurface;
        }

        if (colorMode == LoadTextureColorMode.Desaturate)
        {
            byte* pixels = (byte*)surface->pixels;

            for (var i = 0; i < surface->w * surface->h; i++)
            {
                var r = pixels[i * 4 + 0];
                var g = pixels[i * 4 + 1];
                var b = pixels[i * 4 + 2];

                var t = (byte)(0.299 * r + 0.587 * g + 0.114 * b);

                pixels[i * 4 + 0] = t;
                pixels[i * 4 + 1] = t;
                pixels[i * 4 + 2] = t;
            }
        }
        else if (colorMode == LoadTextureColorMode.WhiteAlpha)
        {
            byte* pixels = (byte*)surface->pixels;

            for (var i = 0; i < surface->w * surface->h; i++)
            {
                pixels[i * 4 + 0] = 255;
                pixels[i * 4 + 1] = 255;
                pixels[i * 4 + 2] = 255;
            }
        }

        if (SDL_PremultiplySurfaceAlpha(surface, true) == false)
        {
            throw new InvalidProgramException("SDL_PremultiplySurfaceAlpha failed");
        }

        var width = surface->w;
        var height = surface->h;
        
        var texture = SDL_CreateTextureFromSurface(renderer, surface);
        SDL_DestroySurface(surface);
        
        return (new SdlHandle<SDL_Texture>(texture), width, height);
    }
    
    public TTextureMap GetMap<TTextureMap>(TextureMaps map) where TTextureMap : class
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(SdlTexture));
        }
        
        return map switch
        {
            TextureMaps.Diffuse => _textureDiff as TTextureMap,
            TextureMaps.GlowMap => _useDiffuseAsGlow ? _textureDiff as TTextureMap : _textureGlow as TTextureMap,
            TextureMaps.DepthBuffer or TextureMaps.StencilBuffer or TextureMaps.NormalMap => throw new NotImplementedException(),
            TextureMaps.None => throw new ArgumentOutOfRangeException(nameof(map), map, null), 
            _ => null
        };
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }
        
        if (_textureDiff != null && _textureDiff.Pointer != null)
        {
            SDL_DestroyTexture(_textureDiff.Pointer);
        }
        
        if (_textureGlow != null && _textureGlow.Pointer != null)
        {
            SDL_DestroyTexture(_textureGlow.Pointer);
        }

        if (_surfaceDiff != null && _surfaceDiff.Pointer != null)
        {
            SDL_DestroySurface(_surfaceDiff.Pointer);
        }
        
        _disposed = true;
        
        GC.SuppressFinalize(this);
    }
}