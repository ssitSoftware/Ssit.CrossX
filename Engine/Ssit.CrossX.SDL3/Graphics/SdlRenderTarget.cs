using Ssit.CrossX.Graphics;
using Ssit.CrossX.SDL3.Common;
using static bottlenoselabs.Interop.SDL;

namespace Ssit.CrossX.SDL3.Graphics;

// ReSharper disable once ClassNeverInstantiated.Global
public unsafe class SdlRenderTarget: IRenderTarget
{
    private bool _disposed;
    private readonly SdlHandle<SDL_Texture> _textureDiff;

    public Size Size { get; }
    public TextureMaps TextureMaps { get; }

    public SdlRenderTarget(SdlHandles handles, CreateRenderTargetParameters parameters)
    {
        Size = parameters.Size;
        TextureMaps = TextureMaps.Diffuse;

        var ptr = SDL_CreateTexture(handles.Renderer, SDL_PixelFormat.SDL_PIXELFORMAT_RGBA8888,
            SDL_TextureAccess.SDL_TEXTUREACCESS_TARGET, Size.Width, Size.Height);
        
        _textureDiff = new SdlHandle<SDL_Texture>(ptr);
    }
    
    public TTextureMap GetMap<TTextureMap>(TextureMaps map) where TTextureMap : class
    {
        return map switch
        {
            TextureMaps.Diffuse => _textureDiff as TTextureMap,
            TextureMaps.GlowMap => null,
            TextureMaps.DepthBuffer or TextureMaps.StencilBuffer or TextureMaps.NormalMap => throw new NotImplementedException(),
            _ => throw new ArgumentOutOfRangeException(nameof(map), map, null)
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

        _disposed = true;
    }
}