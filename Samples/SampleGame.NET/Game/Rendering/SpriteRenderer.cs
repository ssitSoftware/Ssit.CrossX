using System.Numerics;
using Ssit.CrossX.Games.Rendering;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Sprites;

namespace SampleGame.Game.Rendering;

public class SpriteRenderer: IGameObjectRenderer
{
    private readonly SpriteInstance _spriteInstance;
    private ImageTransform _transform = ImageTransform.None;
    
    public event SpriteInstance.SpriteEventDelegate AnimationEvent;
    public event SpriteInstance.SequenceFinishedDelegate AnimationFinished;
    
    public SpriteRenderer(SpriteInstance spriteInstance)
    {
        _spriteInstance = spriteInstance;
        _spriteInstance.SpriteEvent += OnSpriteEvent;
        _spriteInstance.SequenceFinished += OnSequenceFinished;
    }

    private void OnSpriteEvent(string eventName) => AnimationEvent?.Invoke(eventName);
    private void OnSequenceFinished(string sequenceName, bool reverse) => AnimationFinished?.Invoke(sequenceName, reverse);

    public float Scale { get; set; } = 1;

    public virtual void UpdateRenderState(string stateName, ImageTransform transform)
    {
        _transform = transform;
        _spriteInstance.SetSequence(stateName, false);
    }

    public virtual void Animate(float dt, bool reverse)
    {
        _spriteInstance.Advance(dt, reverse);
    }

    public virtual void Render(IRenderer renderer, Vector2 position)
    {
        position *= Scale;
        renderer.DrawSprite(_spriteInstance, position, Scale, null, _transform);
    }
}