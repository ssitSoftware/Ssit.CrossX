using System.Numerics;
using Ssit.CrossX.Graphics;

namespace Ssit.CrossX.Games.Rendering;

public interface IGameObjectRenderer
{
    event AnimationEventDelegate AnimationEvent;
    event AnimationFinishedDelegate AnimationFinished;
    
    float Scale { get; set; }
    void UpdateRenderState(string stateName, ImageTransform transform);
    void Animate(float dt, bool reverse);
    void Render(IRenderer renderer, Vector2 position, RenderPass renderPass);
}

public interface IGameObjectRenderer2
{
    RectangleF Bounds { get; }
    void Render(IRenderer renderer, RenderMode mode);
}