using System;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Values;
using Ssit.CrossX.UI.Views;

namespace RetroGunslinger.Core.UI.Styles;

public static class ButtonStyles
{
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