using System.Numerics;
using Metal;
using Ssit.Pixel.Graphics;

namespace Ssit.Pixel.NET.Graphics;

public interface IMetalShaderEffect: IEffect
{
    void Apply(IMTLRenderCommandEncoder encoder, Matrix4x4? worldTransform = null);
}