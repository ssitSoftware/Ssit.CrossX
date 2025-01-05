using Ssit.CrossX;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.UI.Handlers;
namespace SampleGame.UI.Views;

public class GameViewHandler(ViewHandler.CreateHandlerParameters parameters) : ViewHandler<GameView>(parameters)
{
    public override void Draw(IRenderer renderer)
    {
        renderer.FillRectangle(ScreenBounds, RgbaColor.SaddleBrown);
    }
}