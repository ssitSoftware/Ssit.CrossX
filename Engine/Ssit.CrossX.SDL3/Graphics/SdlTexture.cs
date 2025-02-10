using Interop.Runtime;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.IO;
using Ssit.CrossX.SDL3.Common;
using static bottlenoselabs.Interop.SDL;
using static bottlenoselabs.Interop.SDL_image;

namespace Ssit.CrossX.SDL3.Graphics;

public unsafe class SdlTexture: ITexture
{
    private bool _disposed;
    
    private readonly SdlHandle<SDL_Texture> _textureDiff;
    private readonly SdlHandle<SDL_Texture> _textureGlow;

    public Size Size { get; }
    public TextureMaps TextureMaps { get; }

    public SdlTexture(SdlHandles handles, IFilesProvider filesProvider, string path)
    {
        if (filesProvider.FileExists(path))
        {
            var (texture, width, height) = LoadTexture(filesProvider.Open(path), handles.Renderer);
            Size = new Size(width, height);
            TextureMaps = TextureMaps.Diffuse;
            _textureDiff = texture;
        }
        
        var glowPath = PathHelper.GetPathWithExtension(path, ".glow.png");
        if (filesProvider.FileExists(glowPath))
        {
            var (texture, width, height) = LoadTexture(filesProvider.Open(glowPath), handles.Renderer);
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
            _textureGlow = texture;
        }
    }

    private (SdlHandle<SDL_Texture> ptr, int w, int h) LoadTexture(Stream stream, SDL_Renderer* renderer)
    {
        var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        stream.Close();
        
        SDL_Surface* surface;
        
        var bytes = memoryStream.GetBuffer();
        
        fixed(byte* ptr = bytes)
        {
            var io = SDL_IOFromMem(ptr, (ulong)bytes.Length);
            surface = IMG_Load_IO(io, CBool.FromBoolean(true));
        }

        SDL_PremultiplySurfaceAlpha(surface, CBool.FromBoolean(true));
        
        var width = surface->w;
        var height = surface->h;
        
        var texture = SDL_CreateTextureFromSurface(renderer, surface);
        SDL_DestroySurface(surface);
        
        return (new SdlHandle<SDL_Texture>(texture), width, height);
    }
    
    public TTextureMap GetMap<TTextureMap>(TextureMaps map) where TTextureMap : class
    {
        return map switch
        {
            TextureMaps.Diffuse => _textureDiff as TTextureMap,
            TextureMaps.GlowMap => _textureGlow as TTextureMap,
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

        _disposed = true;
    }
}