using SDL;
using Ssit.CrossX.Graphics;

namespace Ssit.CrossX.SDL.Graphics.Effects;

internal unsafe interface ISdlGpuEffect
{
    SDL_GPURenderState* GetRenderState(SDL_Renderer* renderer);

    /// <summary>
    /// Called right before a quad is drawn with this effect active, with the quad's
    /// destination rectangle in render-coordinate pixels (the same space as the
    /// fragment shader's [[position]] built-in). Lets the effect reconstruct texture
    /// UVs from screen position without relying on SDL's internal vertex-shader
    /// varyings (color/texcoord), whose attribute layout isn't part of the public API.
    /// </summary>
    void PrepareDraw(RectangleF destRect);
}
