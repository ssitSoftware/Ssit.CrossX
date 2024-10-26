using Metal;
using Ssit.Pixel.Graphics;

namespace Ssit.Pixel.NET.Graphics;

public interface IMetalShaderEffect: IEffect
{
    void Apply(IMTLRenderCommandEncoder encoder);
}