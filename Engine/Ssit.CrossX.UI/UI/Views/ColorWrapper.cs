using Ssit.CrossX.Graphics;

namespace Ssit.CrossX.UI.Views;

public readonly struct ColorWrapper
{
    private readonly RgbaColor? _color;
    private readonly int? _colorIndex;

    private ColorWrapper(RgbaColor? color, int? colorIndex)
    {
        _color = color;
        _colorIndex = colorIndex;
    }
    
    public RgbaColor? GetColor(IPaletteSource paletteSource)
    {
        if (_color.HasValue)
        {
            return _color.Value;
        }

        if (_colorIndex.HasValue && paletteSource is not null)
        {
            return paletteSource.Palette[_colorIndex.Value];
        }

        return null;
    }
    
    public static implicit operator ColorWrapper(RgbaColor color) => new(color, null);
    public static implicit operator ColorWrapper(int color) => new(null, color);
}