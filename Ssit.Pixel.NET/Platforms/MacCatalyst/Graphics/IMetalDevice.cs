using Metal;
using MetalKit;
using Ssit.Pixel.Graphics;

namespace Ssit.Pixel.NET.Graphics;

public interface IMetalDevice
{
    MTKView MetalView { get; }
    IMTLCommandBuffer CommandBuffer { get; }
    IRenderTarget RenderTarget { get; }
    Size TargetSize { get; }
}