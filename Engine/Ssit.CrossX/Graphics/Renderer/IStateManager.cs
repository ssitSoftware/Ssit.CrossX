using System.Numerics;

namespace Ssit.CrossX.Graphics.Renderer;

public interface IStateManager
{
    void SaveState();
    void RestoreState();
    void Reset();

    void PushScale(float scale);
    void PushOffset(Vector2 offset);
}