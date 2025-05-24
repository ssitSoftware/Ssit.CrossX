using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;

namespace Ssit.CrossX.UI.Values;

public interface IButtonStateColors
{
    RgbaColor? GetColor(IRenderer2 renderer, IPaletteSource paletteSource, bool hover, bool focused, bool pushed, bool enabled);
}