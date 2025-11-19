using System;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.Graphics.Sprites;
using Ssit.CrossX.XxGames.Physics;
using Ssit.CrossX.XxGames.Platformer.Builders;
using Ssit.CrossX.XxGames.Rendering;
using Ssit.CrossX.XxGames.Utils;

namespace Ssit.CrossX.XxGames.Logic.Objects;

public abstract class SpriteGameObject: StateGameObject, IGameObjectRenderer2, SpriteInstance.IHandler, IBodyOwner
{
    public GameObjectsServices Services { get; }
    public IBody Body { get; }
    public event Action FixedUpdate;
    public SpriteInstance Sprite { get; private set; }
    
    public int ZOrder { get; }
    
    protected ImageTransform Transform { get; set; }
    
    protected RectangleF BoundsRect { get; set; }
    
    public RectangleF Bounds => BoundsRect.Offset(Body.Position);

    void IGameObjectRenderer2.Render(IRenderer2 renderer, RgbaColor color) => OnRender(renderer, color);

    void IBodyOwner.OnFixedUpdate(out bool cancelUpdate)
    {
        cancelUpdate = false;
        OnFixedUpdate(ref cancelUpdate);
    }
    
    void IBodyOwner.OnPostFixedUpdate() => OnPostFixedUpdate();
    void IBodyOwner.OnUpdate(float dt) => OnUpdate(dt);
    
    public bool FaceLeft
    {
        get => Transform == ImageTransform.FlipHorizontal;
        set => Transform = value ? ImageTransform.FlipHorizontal : ImageTransform.None;
    }
    
    protected SpriteGameObject(GameObjectsServices services, ObjectCreationParameters parameters)
    {
        ZOrder = parameters.ZOrder;
        
        Services = services;
        
        Body =  services.Simulation.CreateBody(this);
        Body.Touch();
        Body.Position = parameters.Position;
        
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
        var pos = Body.Position * Services.GameTemplate.TileSize;
        pos = pos.TrimVectorToPixels(Services.GameTemplate.TrimToPixels);
        renderer.SpriteRenderer.Draw(Sprite, pos, transform: Transform, color: color);
    }

    protected virtual void OnFixedUpdate(ref bool cancelUpdate)
    {
        var dt = Services.Simulation.SimulationParameters.TimeDelta;
        OnFixedUpdate(dt);
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