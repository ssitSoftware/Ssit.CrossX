using System.Numerics;

namespace Ssit.CrossX.Graphics.Renderer;

public interface IStateManager
{
    void SaveState();
    void RestoreState();
    void Reset();

    void Scale(float scale);
    void Translate(Vector2 offset);
    void SetBlendMode(BlendMode blendMode);
    void SetTextureFilter(TextureFilter filter);
    void SetGlowMode(bool glowMode);
}