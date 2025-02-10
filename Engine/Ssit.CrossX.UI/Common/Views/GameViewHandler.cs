using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.UI.Handlers;

namespace Ssit.CrossX.Common.Views;

internal class GameViewHandler(ViewHandler.CreateHandlerParameters parameters) : BackgroundHandler<GameView>(parameters)
{
    protected override void OnDraw(IRenderer2 renderer)
    {
        base.OnDraw(renderer);
        
        var simulation = AttachedView.Simulation;
        for (var pass = 0; pass < simulation.RenderPasses; pass++)
        {
            simulation.Render(renderer, ScreenBounds, pass, CurrentScale);
        }
    }

    public override void Update(float dt)
    {
        base.Update(dt);
        
        if (dt > 0)
        {
            var simulation = AttachedView.Simulation;
            simulation.Update(dt);
        }
    }
}