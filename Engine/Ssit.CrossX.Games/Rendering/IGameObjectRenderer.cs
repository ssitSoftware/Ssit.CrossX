using System.Numerics;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Sprites;

namespace Ssit.CrossX.Games.Rendering;

public interface IGameObjectRenderer
{
    event SpriteInstance.SpriteEventDelegate AnimationEvent;
    event SpriteInstance.SequenceFinishedDelegate AnimationFinished;
    
    float Scale { get; set; }
    void UpdateRenderState(string stateName, ImageTransform transform);
    void Animate(float dt, bool reverse);
    void Render(IRenderer renderer, Vector2 position);
}