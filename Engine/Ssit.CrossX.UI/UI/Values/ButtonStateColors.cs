namespace Ssit.CrossX.UI.Values;

public class ButtonStateColors
{
    public RgbaColor? Normal;
    public RgbaColor? Hover;
    public RgbaColor? Focused;
    public RgbaColor? Pushed;
    public RgbaColor? Disabled;

    public RgbaColor? GetColor(bool hover, bool focused, bool pushed, bool enabled)
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
            return Hover ?? Normal;
        }

        if (focused)
        {
            return Focused ?? Normal;
        }

        return Normal;
    }
}