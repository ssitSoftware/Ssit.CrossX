using System.Numerics;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;

namespace Ssit.CrossX.Games.Rendering;

public interface IGameObjectRenderer
{
    event AnimationEventDelegate AnimationEvent;
    event AnimationFinishedDelegate AnimationFinished;
    
    float Scale { get; set; }
    void UpdateRenderState(string stateName, ImageTransform transform);
    void Animate(float dt, bool reverse);
    void Render(IRenderer2 renderer, Vector2 position, RenderPass renderPass);
}