using System.Numerics;

namespace Ssit.CrossX.Graphics;

public struct TextureSpan(ITexture texture, Rectangle? source = null, Vector2? offset = null)
{
    public readonly ITexture Texture = texture;
    public readonly Rectangle? Source = source;
    public readonly Vector2? Offset = offset;
}