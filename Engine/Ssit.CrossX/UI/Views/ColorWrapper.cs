using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.UI.Handlers;

namespace Ssit.CrossX.UI.Views;

public readonly struct ColorWrapper
{
    private readonly RgbaColor? _color;
    private readonly int? _colorIndex;
    private readonly float _opacity;

    public readonly string ColorId;
    
    private ColorWrapper(RgbaColor? color, int? colorIndex, float opacity = 1, string colorId = null)
    {
        _color = color;
        _colorIndex = colorIndex;
        _opacity = opacity;
        ColorId = colorId;
    }
    
    public RgbaColor? GetColor(IPaletteSource paletteSource, IRenderer2 renderer, IColorSource source = null)
    {
        if (ColorId != null)
        {
            var color = source?.GetColor(ColorId);
            if(color.HasValue) return color.Value * _opacity;
        }

        if (_color.HasValue)
        {
            return _color.Value;
        }

        if (_colorIndex.HasValue && paletteSource is not null)
        {
            return renderer.StateManager.IsGlowMode 
                    ? paletteSource.GlowPalette?[_colorIndex.Value] * _opacity ?? RgbaColor.Black
                    : paletteSource.Palette[_colorIndex.Value] * _opacity;
        }

        return null;
    }
    
    public static implicit operator ColorWrapper(RgbaColor color) => new(color, null);
    public static implicit operator ColorWrapper(int color) => new(null, color);
    public static implicit operator ColorWrapper((int color, string colorId) d) => new(null, d.color, colorId: d.colorId);
    public static implicit operator ColorWrapper((int color, float opacity) d) => new(null, d.color, d.opacity);
}