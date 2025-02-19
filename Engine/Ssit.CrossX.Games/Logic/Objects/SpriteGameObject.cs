using System;
using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.Games.Physics.Dynamics;
using Ssit.CrossX.Games.Rendering;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.Graphics.Sprites;

namespace Ssit.CrossX.Games.Logic.Objects;

public abstract class SpriteGameObject: Brain, IGameObjectRenderer2, IDisposable
{
    protected GameObjectsServices Services { get; }
    public Body Body { get; }
    public SpriteInstance Sprite { get; private set; }
    
    public int ZOrder { get; protected set; }
    
    protected ImageTransform Transform { get; set; }
    
    protected RectangleF BoundsRect { get; set; }
    
    public RectangleF Bounds => BoundsRect.Offset(Body.Position);

    void IGameObjectRenderer2.Render(IRenderer2 renderer, RgbaColor color) => OnRender(renderer, color);
    
    protected SpriteGameObject(GameObjectsServices services, ObjectCreationParameters parameters, string path)
    {
        Services = services;
        Body = new Body(services.World);
        Body.BodyType = BodyType.Dynamic;
        Body.Awake = true;
        Body.SetTransform(parameters.Position, 0);
        Body.Owner = this;
        
        Transform = parameters.Flipped ? ImageTransform.FlipHorizontal : ImageTransform.None;
        
        using var go = services.ContentManager.Get<GameObject>(path);
        Sprite = go.Resource.CreateSpriteInstance();
        
        Sprite.SequenceFinished += SpriteOnSequenceFinished;
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

    private void SpriteOnSequenceFinished(SpriteInstance instance, string sequenceName, bool reverse)
    {
        OnAnimationFinished(sequenceName);
    }

    protected override void SetSequence(string state)
    {
        Sprite.SetSequence(state);
    }
    
    public void Dispose()
    {
        Sprite?.Dispose();
        Sprite = null;
    }
}