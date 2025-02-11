using System;
using Ssit.CrossX;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Values;
using Ssit.CrossX.UI.Views;

namespace Gunslinger.Core.UI.Styles;

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
        button.KeyCommandDelay = TimeSpan.FromMilliseconds(180);
        
        button.TextColors = new ButtonStateColors
        {
            Normal = RgbaColor.Gray,
            Hover = RgbaColor.White.Mix(RgbaColor.FromBgra(0xff7ebfc6), 0.5f),
            Focused =  RgbaColor.FromBgra(0xff7ebfc6),
            Pushed = RgbaColor.FromBgra(0xff60969c),
            Disabled = RgbaColor.FromBgra(0xff494949),
            NormalGlow = RgbaColor.Gray * 0.2f,
            HoverGlow = RgbaColor.White.Mix(RgbaColor.FromBgra(0xff7ebfc6), 0.5f) * 0.2f,
            FocusedGlow = RgbaColor.FromBgra(0xff7ebfc6) * 0.7f
        };
        
        button.TextOutlineColors = new ButtonStateColors
        {
            Normal = RgbaColor.Black,
            Hover = RgbaColor.Black,
            Focused = RgbaColor.Black,
            Pushed = RgbaColor.Black,
            Disabled = RgbaColor.Transparent
        };
        
        return button;
    }
}