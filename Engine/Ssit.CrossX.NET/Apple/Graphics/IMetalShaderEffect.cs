#if __IOS__ || __MACCATALYST__

using System.Numerics;
using Metal;
using Ssit.CrossX.Graphics;

namespace Ssit.CrossX.NET.Apple.Graphics;

public interface IMetalShaderEffect: IEffect
{
    void Apply(IMTLRenderCommandEncoder encoder, Matrix4x4? worldTransform = null, RgbaColor? color = null);
}

#endif