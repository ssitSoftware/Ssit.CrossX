using System;
using Nokemono.Core.UI.Views;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Values;
using Ssit.CrossX.UI.Views;

namespace Nokemono.Core.UI.Styles;

public static class ButtonStyles
{
    public static LabelButtonEx WithDialogStyle(this LabelButtonEx button, Align align, bool active)
    {
        button.WithDefaultStyle();

        button.VerticalAlign = Align.Center;
        button.HorizontalAlign = align;
        button.TextAlign = ContentAlign.Center | ContentAlign.VCenter;
        button.Font = ("Default", 12);
        
        button.FocusWaveAmplitude = (button.Font?.FontSize ?? 12) / 9;
        button.FocusWaveFrequency = 1f;
        button.FocusBevel = (button.Font?.FontSize ?? 12) / 10;

        if (!active)
        {
            button.TextColors = new ButtonStateColorsIndexed
            {
                Normal = Palette.Foreground,
                Hover = Palette.Foreground,
                Focused =  Palette.Foreground,
                Pushed = Palette.Foreground,
                Disabled = Palette.Dim
            };
        }
        
        return button;
    }
    public static LabelButton WithDefaultStyle(this LabelButton button)
    {
        button.Scaling = TextScaling.Pixel;
        button.TextAlign = ContentAlign.Center | ContentAlign.VCenter;
        button.VerticalAlign = Align.Center;
        button.HorizontalAlign = Align.Fill;
        button.Height = 22;
        button.Font = ("Default", 18);
        button.Padding = (2, 2);
        button.KeyCommandDelay = TimeSpan.Zero;
        
        button.TextColors = new ButtonStateColorsIndexed
        {
            Normal = Palette.Accent,
            Hover = Palette.Accent,
            Focused =  Palette.Foreground,
            Pushed = Palette.Accent,
            Disabled = Palette.Dim
        };
        
        button.TextOutlineColors = new ButtonStateColorsIndexed
        {
            Normal = Palette.Background,
            Hover = Palette.Background,
            Focused = Palette.Background,
            Pushed = Palette.Background,
            Disabled = Palette.Background
        };
        
        return button;
    }
}