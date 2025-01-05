using System;
using System.Numerics;
using Ssit.CrossX.Games;
using Ssit.CrossX.Games.Rendering;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Sprites;

namespace SampleGame.Game.Rendering;



public class SpriteRenderer: IGameObjectRenderer, IDisposable
{
    private readonly SpriteInstance _spriteInstance;
    private ImageTransform _transform = ImageTransform.None;
    
    public event AnimationEventDelegate AnimationEvent;
    public event AnimationFinishedDelegate AnimationFinished;
    
    public SpriteRenderer(GameObject obj)
    {
        _spriteInstance = obj.CreateSpriteInstance();
        _spriteInstance.SpriteEvent += OnSpriteEvent;
        _spriteInstance.SequenceFinished += OnSequenceFinished;
    }

    private void OnSpriteEvent(SpriteInstance _, string eventName) => AnimationEvent?.Invoke(eventName);
    private void OnSequenceFinished(SpriteInstance _, string sequenceName, bool reverse) => AnimationFinished?.Invoke(sequenceName, reverse);

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

    public virtual void Render(IRenderer renderer, Vector2 position, RenderPass renderPass)
    {
        position *= Scale;

        switch (renderPass)
        {
            case RenderPass.Normal:
                renderer.DrawSprite(_spriteInstance, position, Scale, null, _transform);
                break;
            
            case RenderPass.Shadow:
                break;
        }
    }

    protected virtual void OnDispose()
    {
        _spriteInstance?.Dispose();
    }
    
    public void Dispose()
    {
        OnDispose();
    }
}