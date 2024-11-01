using System.Collections.Generic;
using Ssit.CrossX.Graphics.Effects.Lighting2D;

namespace Ssit.CrossX.Graphics.Effects;

public interface ILightning2dEffect: IEffect
{
    RgbaColor AmbientColor { get; set; }
    
    IList<Light> PointLights { get; }
    IList<SpotLight> SpotLights { get; }
    IList<DirectionalLight> DirectionalLights { get; }
}