using System;
using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.Games.Physics.Dynamics;
using Ssit.CrossX.Games.Rendering;
using Ssit.CrossX.Games.Utils;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.Graphics.Sprites;

namespace Ssit.CrossX.Games.Logic.Objects;

public abstract class SpriteGameObject2 : Updatable, IGameObjectRenderer2, IDisposable
{
    public GameObjectsServices Services { get; }
    public Body Body { get; }
    public int ZOrder { get; }
    
    public SpriteInstance Sprite { get; private set; }
    
    protected ImageTransform Transform { get; set; }
    
    protected RectangleF BoundsRect { get; set; }
    
    public RectangleF Bounds => BoundsRect.Offset(Body.Position);
    
    public bool FaceLeft
    {
        get => Transform == ImageTransform.FlipHorizontal;
        set => Transform = value ? ImageTransform.FlipHorizontal : ImageTransform.None;
    }
    
    void IGameObjectRenderer2.Render(IRenderer2 renderer, RgbaColor color) => OnRender(renderer, color);
    
    internal void CallSpriteEvent(SpriteInstance.Event @event) => OnSpriteEvent(@event);
    internal void CallSequenceFinished(string sequenceName) => OnSequenceFinished(sequenceName);
    
    protected SpriteGameObject2(GameObjectsServices services, ObjectCreationParameters parameters)
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
    }
    
    protected virtual void OnSpriteEvent(SpriteInstance.Event @event)
    {
    }
    
    protected virtual void OnSequenceFinished(string sequenceName)
    {
    }

    protected virtual void OnRender(IRenderer2 renderer, RgbaColor color)
    {
        var pos = Body.Position * Services.GameTemplate.TileSize;
        pos = pos.TrimVectorToPixels(Services.GameTemplate.TrimToPixels);
        renderer.SpriteRenderer.Draw(Sprite, pos, transform: Transform, color: color);
    }

    protected override void OnFixedUpdate(float dt)
    {
        base.OnFixedUpdate(dt);
        Sprite.Advance(dt);
    }
    
    void IDisposable.Dispose()
    {
        OnDispose(true);
    }

    protected virtual void OnDispose(bool disposing)
    {
        if (Sprite is not null)
        {
            Sprite.Handler = null;
            Sprite.Dispose();
            Sprite = null;
        }
    }
}