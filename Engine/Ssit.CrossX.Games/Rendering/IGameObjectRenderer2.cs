using Ssit.CrossX.Graphics.Renderer;

namespace Ssit.CrossX.Games.Rendering;

public interface IGameObjectRenderer2
{
    int ZOrder { get; }
    RectangleF Bounds { get; }
    void Render(IRenderer2 renderer, RgbaColor color);
}