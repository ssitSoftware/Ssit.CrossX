using System;
using System.Collections.Generic;
using Ssit.CrossX.Core;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.Graphics.Sprites;
using Ssit.CrossX.XxGames.Physics;
using Ssit.CrossX.XxGames.Platformer.Builders;
using Ssit.CrossX.XxGames.Rendering;
using Ssit.CrossX.XxGames.Utils;

namespace Ssit.CrossX.XxGames.Logic.Objects;

public abstract class SpriteGameObject2 : IGameObjectRenderer2, IBodyOwner
{
    public GameObjectsServices Services { get; }
    public IBody Body { get; }
    public event Action FixedUpdate;
    public int ZOrder { get; }
    
    public SpriteInstance Sprite { get; private set; }
    
    protected ImageTransform Transform { get; set; }
    
    protected RectangleF BoundsRect { get; set; }

    private readonly List<IUpdatable> _updatables = new();
    
    public RectangleF Bounds => BoundsRect.Offset(Body.Position);
    
    public bool FaceLeft
    {
        get => Transform == ImageTransform.FlipHorizontal;
        set => Transform = value ? ImageTransform.FlipHorizontal : ImageTransform.None;
    }
    
    void IGameObjectRenderer2.Render(IRenderer2 renderer, RgbaColor color) => OnRender(renderer, color);

    void IBodyOwner.OnFixedUpdate(out bool cancelUpdate)
    {
        cancelUpdate = false;
        OnFixedUpdate(ref cancelUpdate);

        var dt = Services.Simulation.SimulationParameters.TimeDelta;
        
        foreach (var updatable in _updatables)
        {
            updatable.FixedUpdate(dt);
        }
    }

    void IBodyOwner.OnPostFixedUpdate()
    {
        OnPostFixedUpdate();
        
        foreach (var updatable in _updatables)
        {
            updatable.PostFixedUpdate();
        }
    }

    void IBodyOwner.OnUpdate(float time)
    {
        OnUpdate(time);
        
        foreach (var updatable in _updatables)
        {
            updatable.Update(time);
        }
    }
    
    internal void CallSpriteEvent(SpriteInstance.Event @event) => OnSpriteEvent(@event);
    internal void CallSequenceFinished(string sequenceName) => OnSequenceFinished(sequenceName);

    internal void AddUpdatableInternal(IUpdatable updatable) => _updatables.Add(updatable);

    protected SpriteGameObject2(GameObjectsServices services, ObjectCreationParameters parameters)
    {
        ZOrder = parameters.ZOrder;
        
        Services = services;

        Body = services.Simulation.CreateBody(this);

        Body.Touch();
        Body.Position = parameters.Position;
        
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

    protected virtual void OnFixedUpdate(ref bool cancelUpdate)
    {
        var dt = Services.Simulation.SimulationParameters.TimeDelta;
        Sprite.Advance(dt);
    }

    protected virtual void OnPostFixedUpdate()
    {
    }
    
    protected virtual void OnUpdate(float dt)
    {
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