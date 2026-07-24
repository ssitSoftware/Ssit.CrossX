using System;
using System.Collections.Generic;

namespace Ssit.CrossX.Graphics.Utils;

public static class ColorAdjustments
{
    public static RgbaColor[] AdjustPalette(IReadOnlyList<RgbaColor> colors, float hueShift = 0, float saturationMultiply = 1,
        float lightnessMultiply = 1, float gamma = 1, bool premultiplied = true)
    {
        var newColors = new RgbaColor[colors.Count];
        for (var idx = 0; idx < colors.Count; idx++)
        {
            var color = colors[idx];
            if (premultiplied)
            {
                color = color.ToNonPremultiplied();
            }

            newColors[idx] = AdjustGamma(AdjustHsl(color, hueShift, saturationMultiply, lightnessMultiply, false),
                gamma, false).AsPremultiplied();
        }

        return newColors;
    }
    
    public static RgbaColor[] ToTechnicolor(IReadOnlyList<RgbaColor> colors, bool premultiplied = true)
    {
        var newColors = new RgbaColor[colors.Count];
        for (var idx = 0; idx < colors.Count; idx++)
        {
            var color = colors[idx];
            if (premultiplied)
            {
                color = color.ToNonPremultiplied();
            }
            color = AdjustHsl(AdjustGamma(color, 1.5f, false), 0, 0.95f, 1, false);
            newColors[idx] = Technicolor.ToTechniColor2(color);
            newColors[idx] = newColors[idx].Mix(colors[idx], 0.5f);
        }

        return newColors;
    }

    private static RgbaColor ToNonPremultiplied(this RgbaColor color)
    {
        if (color.A == 0)
            return color;
        
        return new RgbaColor(color.Rf / color.Af, color.Gf / color.Af, color.Bf / color.Af, color.Af);
    }
    
    public static RgbaColor AdjustGamma(RgbaColor color, float gamma, bool premultiplied = true)
    {
        if (premultiplied)
        {
            color = color.ToNonPremultiplied();
        }

        var pow = 1f / gamma;
        var result = new RgbaColor(MathF.Pow(color.Rf, pow), MathF.Pow(color.Gf, pow), MathF.Pow(color.Bf, pow));  
        
        return premultiplied ? result * color.Af : result.WithOpacity(color.Af);
    }
    
    public static RgbaColor AdjustHsl(RgbaColor color, float hueShift = 0, float saturationMultiply = 1, float lightnessMultiply = 1, bool premultiplied = true)
    {
        if (premultiplied)
        {
            color = color.ToNonPremultiplied();
        }

        var (h, s, l) = color.ToHsl();
        h = (h + hueShift + 360) % 360;
        s *= saturationMultiply;
        l *= lightnessMultiply;

        var result = RgbaColor.FromHsl(h, s, l);
        return premultiplied ? result * color.Af : result.WithOpacity(color.Af);
    }
}