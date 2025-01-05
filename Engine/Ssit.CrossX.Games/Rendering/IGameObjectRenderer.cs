using System.Numerics;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Sprites;

namespace Ssit.CrossX.Games.Rendering;

public delegate void AnimationEventDelegate(string eventName);
public delegate void AnimationFinishedDelegate(string sequenceName, bool reverse);


public interface IGameObjectRenderer
{
    event AnimationEventDelegate AnimationEvent;
    event AnimationFinishedDelegate AnimationFinished;
    
    float Scale { get; set; }
    void UpdateRenderState(string stateName, ImageTransform transform);
    void Animate(float dt, bool reverse);
    void Render(IRenderer renderer, Vector2 position, RenderPass renderPass);
}