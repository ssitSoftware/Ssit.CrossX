using Ssit.CrossX;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Views;

namespace SampleGame.Game.UI.Styles;

public static class ButtonStyles
{
    public static LabelButton WithDefaultStyle(this LabelButton button)
    {
        button.TextAlign = ContentAlign.Center | ContentAlign.VCenter;
        button.VerticalAlign = Align.Start;
        button.HorizontalAlign = Align.Center;
        button.BackgroundColor = RgbaColor.Chartreuse;
        button.Height = 50;
        button.Font = ("Default", 16);
        button.Padding = (16, 16);
        button.TextColor = RgbaColor.DarkGray;
        button.HoverTextColor = RgbaColor.LightGray;
        button.FocusedTextColor = RgbaColor.Yellow;
        button.DisabledTextColor = new(0xff494949);
        return button;
    }
}