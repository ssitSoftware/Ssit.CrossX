using Ssit.CrossX;
using Ssit.CrossX.Games.Rendering;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.UI.Handlers;
namespace SampleGame.UI.Views;

public class GameViewHandler(ViewHandler.CreateHandlerParameters parameters) : ViewHandler<GameView>(parameters)
{
    public override void Draw(IRenderer renderer)
    {
        renderer.FillRectangle(ScreenBounds, RgbaColor.SaddleBrown);

        var simulation = AttachedView.Simulation;
        
        simulation.Render(renderer, ScreenBounds, RenderPass.Background, CurrentScale);
        simulation.Render(renderer, ScreenBounds, RenderPass.Shadow, CurrentScale);
        simulation.Render(renderer, ScreenBounds, RenderPass.Normal, CurrentScale);
        simulation.Render(renderer, ScreenBounds, RenderPass.Overlay, CurrentScale);
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