using System.Collections.Generic;
using System.Numerics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.XxGames.Rendering;

namespace Ssit.CrossX.Games.TopDown.Rendering;

public interface IGameObjectRenderer
{
    event AnimationEventDelegate AnimationEvent;
    event AnimationFinishedDelegate AnimationFinished;
    
    void Animate(float dt, bool reverse);
    void Render(IRenderer2 renderer, Vector2 position, RenderPass renderPass);
}

public interface IGameObjectVertexRenderer
{
    void PrepareVertices(IList<Vertex> vertices);
}