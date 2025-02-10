using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;

namespace Ssit.CrossX.UI.Values;

public class ButtonStateColors
{
    public RgbaColor? Normal;
    public RgbaColor? Hover;
    public RgbaColor? Focused;
    public RgbaColor? Pushed;
    public RgbaColor? Disabled;
    
    public RgbaColor? NormalGlow;
    public RgbaColor? HoverGlow;
    public RgbaColor? FocusedGlow;
    public RgbaColor? PushedGlow;
    public RgbaColor? DisabledGlow;

    public RgbaColor? GetColor(IRenderer2 renderer, bool hover, bool focused, bool pushed, bool enabled)
    {
        if (renderer.StateProvider.UseGlowTextures)
        {
            return GetColorGlow(hover, focused, pushed, enabled);
        }
        
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
    
    private RgbaColor? GetColorGlow(bool hover, bool focused, bool pushed, bool enabled)
    {
        if (!enabled)
        {
            return DisabledGlow ?? (Disabled?.A > 0 ? RgbaColor.Black : null);
        }

        if (pushed)
        {
            return PushedGlow ?? (Pushed?.A > 0 ? RgbaColor.Black : null);
        }

        if (hover)
        {
            return HoverGlow ?? (Hover?.A > 0 ? RgbaColor.Black : null);
        }

        if (focused)
        {
            return FocusedGlow ?? (Focused?.A > 0 ? RgbaColor.Black : null);
        }

        return NormalGlow ?? (Normal?.A > 0 ? RgbaColor.Black : null);
    }
}