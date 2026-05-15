using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI.Handlers;

public class FrameHandler(ViewHandler.CreateHandlerParameters parameters, IPaletteSource paletteSource) : BackgroundHandler<Frame>(parameters, paletteSource)
{
    protected virtual RgbaColor? FrameColor(IRenderer2 renderer) => AttachedView.FrameColor.GetColor(PaletteSource, renderer);
    
    protected override void OnDraw(IRenderer2 renderer)
    {
        base.OnDraw(renderer);
        
        var frameColor = FrameColor(renderer);
        var frameWidth = AttachedView.FrameWidth?.Calculate(CurrentScale, ScreenBounds.Width) ?? CurrentScale;
        
        if (frameColor.HasValue)
        {
            var topRect =  new RectangleF(ScreenBounds.X, ScreenBounds.Y, ScreenBounds.Width, frameWidth);
            renderer.GeometryRenderer.DrawRectangle(topRect, frameColor.Value);
            
            var bottomRect = new RectangleF(ScreenBounds.X, ScreenBounds.Bottom - frameWidth, ScreenBounds.Width, frameWidth);
            renderer.GeometryRenderer.DrawRectangle(bottomRect, frameColor.Value);
            
            var leftRect = new RectangleF(ScreenBounds.X, ScreenBounds.Y + frameWidth, frameWidth, ScreenBounds.Height - frameWidth * 2);
            renderer.GeometryRenderer.DrawRectangle(leftRect, frameColor.Value);
            
            var rightRect = new RectangleF(ScreenBounds.Right - frameWidth, ScreenBounds.Y + frameWidth, frameWidth, ScreenBounds.Height - frameWidth * 2);
            renderer.GeometryRenderer.DrawRectangle(rightRect, frameColor.Value);
        }
    }
}