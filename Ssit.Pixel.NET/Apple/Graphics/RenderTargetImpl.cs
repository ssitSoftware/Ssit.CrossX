#if __IOS__ || __MACCATALYST__

using Metal;
using Ssit.Pixel.Graphics;

namespace Ssit.Pixel.NET.Apple.Graphics;

public class RenderTargetImpl: IRenderTarget
{
    private IMTLTexture _texture;
    private IMTLTexture _depthTexture;
    
    public Size Size { get; }
    public TextureMaps TextureMaps { get; }

    public RenderTargetImpl(IMetalDevice device, CreateRenderTargetParameters parameters)
    {
        Size = parameters.Size;
        TextureMaps = TextureMaps.Diffuse | TextureMaps.DepthBuffer | TextureMaps.StencilBuffer;
        
        _texture = device.MetalView.Device!.CreateTexture(new MTLTextureDescriptor
        {
            Width = (uint)parameters.Size.Width,
            Height = (uint)parameters.Size.Height,
            ArrayLength = 1,
            StorageMode = MTLStorageMode.Shared,
            Usage = MTLTextureUsage.RenderTarget | MTLTextureUsage.ShaderRead,
            PixelFormat = device.MetalView.ColorPixelFormat
        });
        
        _depthTexture = device.MetalView.Device!.CreateTexture(new MTLTextureDescriptor
        {
            Width = (uint)parameters.Size.Width,
            Height = (uint)parameters.Size.Height,
            ArrayLength = 1,
            StorageMode = MTLStorageMode.Private,
            Usage = MTLTextureUsage.RenderTarget  | MTLTextureUsage.ShaderRead,
            PixelFormat = device.MetalView.DepthStencilPixelFormat
        });
    }
    
    public void Dispose()
    {
        _texture?.Dispose();
        _texture = null;
        
        _depthTexture?.Dispose();
        _depthTexture = null;
    }
    
    public TTextureMap GetMap<TTextureMap>(TextureMaps map) where TTextureMap : class
    {
        switch (map)
        {
            case TextureMaps.Diffuse:
                return _texture as TTextureMap;
            
            case TextureMaps.DepthBuffer:
            case TextureMaps.StencilBuffer:
                return _depthTexture as TTextureMap;
        }
        return null;
    }
}

#endif