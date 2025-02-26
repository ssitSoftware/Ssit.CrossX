using System;
using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.Games.Physics.Dynamics;
using Ssit.CrossX.Games.Rendering;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.Graphics.Sprites;

namespace Ssit.CrossX.Games.Logic.Objects;

public abstract class SpriteGameObject: Brain, IGameObjectRenderer2, SpriteInstance.IHandler, IDisposable
{
    protected GameObjectsServices Services { get; }
    public Body Body { get; }
    public SpriteInstance Sprite { get; private set; }
    
    public int ZOrder { get; }
    
    protected ImageTransform Transform { get; set; }
    
    protected RectangleF BoundsRect { get; set; }
    
    public RectangleF Bounds => BoundsRect.Offset(Body.Position);

    void IGameObjectRenderer2.Render(IRenderer2 renderer, RgbaColor color) => OnRender(renderer, color);
    
    protected SpriteGameObject(GameObjectsServices services, ObjectCreationParameters parameters)
    {
        ZOrder = parameters.ZOrder;
        
        Services = services;
        Body = new Body(services.World);
        Body.BodyType = BodyType.Dynamic;
        Body.Awake = true;
        Body.SetTransform(parameters.Position, 0);
        Body.Owner = this;
        
        Transform = parameters.Flipped ? ImageTransform.FlipHorizontal : ImageTransform.None;
    }

    protected void InitializeSprite(string spritePath)
    {
        using var go = Services.ContentManager.Get<GameObject>(spritePath);
        Sprite = go.Resource.CreateSpriteInstance();
        Sprite.Handler = this;
    }

    protected virtual void OnSpriteEvent(SpriteInstance instance, SpriteInstance.Event @event)
    {
    }

    protected virtual void OnRender(IRenderer2 renderer, RgbaColor color)
    {
        renderer.SpriteRenderer.Draw(Sprite, Body.Position * Services.GameTemplate.TileSize, transform: Transform, color: color);
    }

    protected override void OnFixedUpdate(float dt)
    {
        base.OnFixedUpdate(dt);
        Sprite.Advance(dt);
    }

    protected override void SetSequence(string state)
    {
        Sprite.SetSequence(state);
    }
    
    void IDisposable.Dispose()
    {
        OnDispose(true);
    }

    protected virtual void OnDispose(bool disposing)
    {
        Sprite?.Dispose();
        Sprite = null;
    }

    void SpriteInstance.IHandler.OnSpriteEvent(SpriteInstance instance, SpriteInstance.Event @event) => OnSpriteEvent(instance, @event);

    void SpriteInstance.IHandler.OnSequenceFinished(SpriteInstance instance, string sequenceName, bool reverse) => OnAnimationFinished(sequenceName);
}