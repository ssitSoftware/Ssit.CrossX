using System.Numerics;

namespace Ssit.CrossX.Graphics.Renderer;

public interface IRendererStateManager
{
    void SaveState();
    void RestoreState();
    void ResetState();

    void Scale(float scale);
    void Translate(Vector2 offset);
    void ClipRect(RectangleF rect);
}