#if __IOS__ || __MACCATALYST__

using System.Numerics;
using Metal;
using Ssit.Pixel.Graphics;

namespace Ssit.Pixel.NET.Apple.Graphics;

public interface IMetalShaderEffect: IEffect
{
    void Apply(IMTLRenderCommandEncoder encoder, Matrix4x4? worldTransform = null);
}

#endif