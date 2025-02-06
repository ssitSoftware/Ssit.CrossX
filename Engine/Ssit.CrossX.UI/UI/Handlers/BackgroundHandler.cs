using Ssit.CrossX.Graphics;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI.Handlers;

public class BackgroundHandler<TBackground>(ViewHandler.CreateHandlerParameters parameters) : ViewHandler<TBackground>(parameters) where TBackground: Background
{
    protected virtual RgbaColor? BackgroundColor(RenderMode mode) => mode == RenderMode.Normal
        ? AttachedView.BackgroundColor
        : RgbaColor.Black * (AttachedView.BackgroundColor?.Af ?? 1f);
    
    protected override void OnDraw(IRenderer renderer, RenderMode mode)
    {
        var bgColor = BackgroundColor(mode);
        if (bgColor.HasValue)
        {
            renderer.FillRectangle(ScreenBounds, bgColor.Value);
        }
    }
}

public class BackgroundHandler(ViewHandler.CreateHandlerParameters parameters) : BackgroundHandler<Background>(parameters)
{
}