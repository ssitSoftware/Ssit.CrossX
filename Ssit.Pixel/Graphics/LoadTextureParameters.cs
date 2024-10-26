using System.IO;

namespace Ssit.Pixel.Graphics;

public class LoadTextureParameters
{
    public Stream DiffuseMapStream { get; set; }
    public Stream NormalMapStream { get; set; }
}