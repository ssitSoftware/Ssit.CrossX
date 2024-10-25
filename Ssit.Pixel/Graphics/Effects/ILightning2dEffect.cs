using System.Collections.Generic;
using Ssit.Pixel.Graphics.Effects.Lighting2D;

namespace Ssit.Pixel.Graphics.Effects;

public interface ILightning2dEffect: IEffect
{
    RgbaColor AmbientColor { get; set; }
    
    IList<Light> PointLights { get; }
    IList<SpotLight> SpotLights { get; }
    IList<DirectionalLight> DirectionalLights { get; }
}