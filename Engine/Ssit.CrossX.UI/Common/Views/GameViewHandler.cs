using Ssit.CrossX.Graphics;
using Ssit.CrossX.UI.Handlers;

namespace Ssit.CrossX.Common.Views;

internal class GameViewHandler(ViewHandler.CreateHandlerParameters parameters, IRenderModeProvider renderModeProvider) : BackgroundHandler<GameView>(parameters, renderModeProvider)
{
    protected override void OnDraw(IRenderer renderer)
    {
        base.OnDraw(renderer);
        
        var simulation = AttachedView.Simulation;

        for (var pass = 0; pass < simulation.RenderPasses; pass++)
        {
            simulation.Render(renderer, ScreenBounds, pass, CurrentScale);
        }
    }

    protected override void OnDrawDebug(IRenderer renderer)
    {
        var simulation = AttachedView.Simulation;
        simulation.RenderDebug(renderer, ScreenBounds, CurrentScale);
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