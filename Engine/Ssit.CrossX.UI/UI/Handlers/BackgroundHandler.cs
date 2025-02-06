using Ssit.CrossX.Graphics;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI.Handlers;

public class BackgroundHandler<TBackground>(ViewHandler.CreateHandlerParameters parameters, IRenderModeProvider renderModeProvider) : ViewHandler<TBackground>(parameters, renderModeProvider) where TBackground: Background
{
    protected virtual RgbaColor? BackgroundColor => AttachedView.BackgroundColor;
    
    protected override void OnDraw(IRenderer renderer)
    {
        if (BackgroundColor.HasValue)
        {
            renderer.FillRectangle(ScreenBounds, BackgroundColor.Value);
        }
    }
}

public class BackgroundHandler(ViewHandler.CreateHandlerParameters parameters, IRenderModeProvider renderModeProvider) : BackgroundHandler<Background>(parameters, renderModeProvider)
{
}