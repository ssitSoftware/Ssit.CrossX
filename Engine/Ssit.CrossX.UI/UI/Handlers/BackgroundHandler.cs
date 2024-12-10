using Ssit.CrossX.Graphics;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI.Handlers;

public class BackgroundHandler<TBackground>(ViewHandler.CreateHandlerParameters parameters) : ViewHandler<TBackground>(parameters) where TBackground: Background
{
    public override void Draw(IRenderer renderer)
    {
        if (AttachedView.BackgroundColor.HasValue)
        {
            renderer.FillRectangle(ScreenBounds, AttachedView.BackgroundColor.Value);
        }
    }
}

public class BackgroundHandler(ViewHandler.CreateHandlerParameters parameters) : BackgroundHandler<Background>(parameters)
{
}