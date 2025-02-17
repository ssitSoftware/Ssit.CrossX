using Ssit.CrossX.Graphics.Renderer;

namespace Ssit.CrossX.Games.Rendering;

public interface IGameObjectRenderer2
{
    RectangleF Bounds { get; }
    void Render(IRenderer2 renderer);
}