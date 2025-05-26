using System;
using Nokemono.Core.UI.Views;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Values;
using Ssit.CrossX.UI.Views;

namespace Nokemono.Core.UI.Styles;

public static class ButtonStyles
{
    public static LabelButtonEx WithDialogStyle(this LabelButtonEx button)
    {
        button.WithDefaultStyle();

        button.VerticalAlign = Align.Center;
        button.HorizontalAlign = Align.Center;
        button.Width = "100%";

        button.TextAlign = ContentAlign.Center;
        button.Font = ("Default", 12);
        
        button.FocusWaveAmplitude = (button.Font?.FontSize ?? 12) / 6;
        button.FocusWaveFrequency = 1f;
        button.FocusBevel = (button.Font?.FontSize ?? 12) / 6;
        
        button.Padding = (0, 4, 0, 0);

        button.FocusedLowColor = 1;
        button.TextColors = new ButtonStateColorsIndexed
        {
            Normal = Palette.ButtonAccent,
            Hover = Palette.Background,
            Focused =  Palette.Foreground,
            Pushed = Palette.ButtonAccent,
            Disabled = Palette.Dim
        };
        
        button.BackgroundColors = new ButtonStateColorsIndexed
        {
            Normal = Palette.Background,
            Hover = Palette.Background,
            Focused =  Palette.ButtonAccent,
            Pushed = Palette.ButtonAccent,
            Disabled = Palette.Background
        };
        
        return button;
    }
    public static LabelButtonEx WithDefaultStyle(this LabelButtonEx button)
    {
        button.Scaling = TextScaling.Pixel;
        button.TextAlign = ContentAlign.Center | ContentAlign.VCenter;
        button.VerticalAlign = Align.Center;
        button.HorizontalAlign = Align.Fill;
        button.Height = 22;
        button.Font = ("Default", 18);
        button.Padding = (2, 2);
        button.KeyCommandDelay = TimeSpan.Zero;
        
        button.FocusWaveAmplitude = (button.Font?.FontSize ?? 12) / 9;
        button.FocusWaveFrequency = 1f;
        button.FocusBevel = (button.Font?.FontSize ?? 12) / 8;
        
        button.TextColors = new ButtonStateColorsIndexed
        {
            Normal = Palette.ButtonAccent,
            Hover = Palette.Background,
            Focused =  Palette.Foreground,
            Pushed = Palette.ButtonAccent,
            Disabled = Palette.Dim
        };
        
        // button.BackgroundColors = new ButtonStateColorsIndexed
        // {
        //     Normal = Palette.Background,
        //     Hover = Palette.Background,
        //     Focused =  Palette.Background,
        //     Pushed = Palette.Background,
        //     Disabled = Palette.Background
        // };
        
        return button;
    }
}