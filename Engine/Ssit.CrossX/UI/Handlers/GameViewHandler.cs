using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI.Handlers;

internal class GameViewHandler(ViewHandler.CreateHandlerParameters parameters, IPaletteSource paletteSource = null) : BackgroundHandler<GameView>(parameters, paletteSource)
{
    protected override void OnDraw(IRenderer2 renderer)
    {
        base.OnDraw(renderer);
        
        var gameInstance = AttachedView.GameInstance;
        for (var pass = 0; pass < gameInstance.RenderPasses; pass++)
        {
            gameInstance.Render(renderer, ScreenBounds, pass, CurrentScale);
        }

        if (AttachedView.ShowDebug?.Value ?? false)
        {
            gameInstance.RenderDebug(renderer, ScreenBounds, CurrentScale);
        }
    }

    public override void Update(float dt)
    {
        base.Update(dt);
        
        if (dt > 0 && (AttachedView.Active?.Value ?? true))
        {
            var gameInstance = AttachedView.GameInstance;
            gameInstance.Update(dt * AttachedView.SpeedFactor);
        }
    }
}