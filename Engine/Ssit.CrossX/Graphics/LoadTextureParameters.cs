using System.IO;

namespace Ssit.CrossX.Graphics;

public class LoadTextureParameters
{
    public Stream DiffuseMapStream { get; set; }
    public Stream NormalMapStream { get; set; }
    public Stream GlowMapStream { get; set; }
}