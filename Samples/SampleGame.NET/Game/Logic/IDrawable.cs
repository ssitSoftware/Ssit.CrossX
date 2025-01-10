using Ssit.CrossX.Games.Rendering;
using Ssit.CrossX.Graphics;

namespace SampleGame.Game.Logic;

public interface IDrawable
{
    void Draw(IRenderer renderer, RenderPass renderPass);
}