using System;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;

namespace Ssit.CrossX.UI.Values;

public class ButtonStateColorsIndexed : IButtonStateColors
{
    public byte? Normal;
    public byte? Hover;
    public byte? Focused;
    public byte? Pushed;
    public byte? Disabled;
    
    public RgbaColor? GetColor(IRenderer2 renderer, IPaletteSource paletteSource, bool hover, bool focused, bool pushed, bool enabled)
    {
        if (paletteSource is null)
            throw new InvalidOperationException();
        
        var index = GetIndex(hover, focused, pushed, enabled) ?? 2;
        
        if (renderer.StateProvider.UseGlowTextures)
        {
            if (paletteSource.GlowPalette is null)
                return RgbaColor.Black;
            
            return paletteSource.GlowPalette[index];
        }
        
        return paletteSource.Palette[index];
    }

    private int? GetIndex(bool hover, bool focused, bool pushed, bool enabled)
    {
        if (!enabled)
        {
            return Disabled ?? Normal;
        }

        if (pushed)
        {
            return Pushed ?? Focused ?? Hover ?? Normal;
        }

        if (hover)
        {
            return Hover ?? (focused ? Focused ?? Normal : Normal);
        }

        if (focused)
        {
            return Focused ?? Normal;
        }

        return Normal;
    }
}