using System.Numerics;

namespace Ssit.CrossX.Graphics.Renderer;

public interface IRenderStateProvider
{
    float Scale { get; }
    Vector2 Offset { get; }
    bool UseGlowTextures { get; }
    BlendMode BlendMode { get; }
    TextureFilter TextureFilter { get; }
    RectangleF? ClipRect { get; }
}