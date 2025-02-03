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
        button.PixelScaling = true;
        button.TextAlign = ContentAlign.Center | ContentAlign.VCenter;
        button.VerticalAlign = Align.Center;
        button.HorizontalAlign = Align.Fill;
        button.Height = 22;
        button.Font = ("Default", 18);
        button.Padding = (2, 2);
        
        button.TextColors = new ButtonStateColors
        {
            Normal = RgbaColor.Gray,
            Hover = RgbaColor.White.Mix(RgbaColor.FromBgra(0xff7ebfc6), 0.5f),
            Focused =  RgbaColor.FromBgra(0xff7ebfc6),
            Pushed = RgbaColor.FromBgra(0xff60969c),
            Disabled = RgbaColor.FromBgra(0xff494949)
        };
        button.TextOutlineColors = new ButtonStateColors
        {
            Normal = RgbaColor.Black,
            Hover = RgbaColor.Black,
            Focused = RgbaColor.Black,
            Pushed = RgbaColor.Black,
            Disabled = RgbaColor.Transparent
        };
        // button.BackgroundColors = new ButtonStateColors
        // {
        //     Normal = RgbaColor.FromNonPremultiplied(255,255,255,20),
        //     Hover = RgbaColor.FromNonPremultiplied(255,255,255,30),
        //     Focused = RgbaColor.FromNonPremultiplied(255,255,255,45),
        //     Pushed = RgbaColor.FromNonPremultiplied(255,235,215,60),
        //     Disabled = RgbaColor.FromNonPremultiplied(0,0,0,10),
        // };
        return button;
    }
}