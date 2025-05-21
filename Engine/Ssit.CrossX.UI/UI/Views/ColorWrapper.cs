using Ssit.CrossX.Graphics;

namespace Ssit.CrossX.UI.Views;

public readonly struct ColorWrapper
{
    private readonly RgbaColor? _color;
    private readonly int? _colorIndex;
    private readonly float _opacity;

    private ColorWrapper(RgbaColor? color, int? colorIndex, float opacity = 1)
    {
        _color = color;
        _colorIndex = colorIndex;
        _opacity = opacity;
    }
    
    public RgbaColor? GetColor(IPaletteSource paletteSource)
    {
        if (_color.HasValue)
        {
            return _color.Value;
        }

        if (_colorIndex.HasValue && paletteSource is not null)
        {
            return paletteSource.Palette[_colorIndex.Value] * _opacity;
        }

        return null;
    }
    
    public static implicit operator ColorWrapper(RgbaColor color) => new(color, null);
    public static implicit operator ColorWrapper(int color) => new(null, color);
    
    public static implicit operator ColorWrapper((int color, float opacity) d) => new(null, d.color, d.opacity);
}