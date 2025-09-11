using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI.Handlers;

public class BackgroundHandler<TBackground>(ViewHandler.CreateHandlerParameters parameters, IPaletteSource paletteSource) 
    : ViewHandler<TBackground>(parameters) where TBackground: Background
{
    protected IPaletteSource PaletteSource { get; } = paletteSource;

    protected virtual RgbaColor? BackgroundColor(IRenderer2 renderer) => renderer.StateProvider.UseGlowTextures
        ? RgbaColor.Black * (AttachedView.BackgroundColor.GetColor(PaletteSource)?.Af ?? 1f)
        : AttachedView.BackgroundColor.GetColor(PaletteSource);
    
    protected override void OnDraw(IRenderer2 renderer)
    {
        var bgColor = BackgroundColor(renderer);
        if (bgColor.HasValue)
        {
            renderer.GeometryRenderer.FillRectangle(ScreenBounds, bgColor.Value);
        }
    }
}

public class BackgroundHandler(ViewHandler.CreateHandlerParameters parameters, IPaletteSource paletteSource = null) 
    : BackgroundHandler<Background>(parameters, paletteSource)
{
}