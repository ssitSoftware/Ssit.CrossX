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
        button.VerticalAlign = Align.Center;
        button.HorizontalAlign = Align.Center;
        button.Font = ("Default", 24);
        button.Padding = (0, 16);
        button.TextColor = RgbaColor.DarkGray;
        button.HoverTextColor = RgbaColor.LightGray;
        button.FocusedTextColor = RgbaColor.Yellow;
        button.DisabledTextColor = new(0xff494949);
        return button;
    }
}