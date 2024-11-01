#if __IOS__ || __MACCATALYST__

using Metal;
using MetalKit;
using Ssit.CrossX.Graphics;

namespace Ssit.CrossX.NET.Apple.Graphics;

public interface IMetalDevice
{
    MTKView MetalView { get; }
    IMTLCommandBuffer CommandBuffer { get; }
    IRenderTarget CurrentRenderTarget { get; set; }
    Size TargetSize { get; }
    void CommitCommandBuffer();
}

#endif