using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI.Handlers;

public class FrameHandler(ViewHandler.CreateHandlerParameters parameters, IPaletteSource paletteSource) : BackgroundHandler<Frame>(parameters, paletteSource)
{
    protected virtual RgbaColor? FrameColor(IRenderer2 renderer) => renderer.StateProvider.UseGlowTextures
        ? RgbaColor.Black * (AttachedView.FrameColor.GetColor(PaletteSource)?.Af ?? 1f)
        : AttachedView.FrameColor.GetColor(PaletteSource);
    
    protected override void OnDraw(IRenderer2 renderer)
    {
        base.OnDraw(renderer);
        
        var frameColor = FrameColor(renderer);
        if (frameColor.HasValue)
        {
            renderer.GeometryRenderer.DrawRectangle(ScreenBounds, frameColor.Value);
        }
    }
}