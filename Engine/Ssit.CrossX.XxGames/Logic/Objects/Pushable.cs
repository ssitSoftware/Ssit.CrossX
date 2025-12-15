using System;
using System.Numerics;
using Ssit.CrossX.Content;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.Graphics.Sprites;
using Ssit.CrossX.XxGames.Physics;
using Ssit.CrossX.XxGames.Physics.Coliders;
using Ssit.CrossX.XxGames.Platformer.Builders;
using Ssit.CrossX.XxGames.Rendering;
using Ssit.CrossX.XxGames.Utils;

namespace Ssit.CrossX.XxGames.Logic.Objects;

public class Pushable(GameObjectsServices services, ObjectCreationParameters parameters): IGameObjectRenderer2, IBodyOwner, IPushable
{
    protected virtual bool CanPull => false;
    bool IPushable.CanPull => CanPull;
    
    void IGameObjectRenderer2.Render(IRenderer2 renderer, RgbaColor color) => Render(renderer, color);
    RectangleF IGameObjectRenderer2.Bounds => BoundsRect.Offset(Body.Position);
    int IGameObjectRenderer2.ZOrder { get; } = parameters.ZOrder;
    
    void IBodyOwner.OnFixedUpdate(out bool cancelUpdate)
    {
        cancelUpdate = false;
        FixedUpdate?.Invoke();
    }
    
    protected GameObjectsServices Services { get; } = services;
    
    public event Action FixedUpdate;
    public IBody Body { get; private set; }
    protected RectangleF BoundsRect { get; set; }
    
    private ResourceHandle<ITexture> _spriteSheet;
    private Sprite.SpriteSequence _sequence;
    
    protected Vector2 Origin { get; private set; }
    
    protected void InitializeSprite(string spritePath)
    {
        using var go = Services.ContentManager.Get<SpriteEx>(spritePath);
        var sprite = go.Resource.Sprite;

        Origin = go.Resource.Description.Origin;
        _spriteSheet = Services.ContentManager.Get<ITexture>(sprite.SheetName);
        _sequence = sprite.GetSequence("Default");
    }
    
    protected void InitializePhysics(SizeF size, IMaterial material)
    {
        var simulation = Services.Simulation;
        
        Body = simulation.CreateBody(this);
        Body.Position  = parameters.Position;
        
        Body.AddColliders(simulation.CreateCollider(new RectColliderCreationParameters
        {
            Active = true,
            AttachToBody = Body,
            Center = Vector2.Zero,
            Size = size,
            Type = ColliderType.Dynamic,
            Material = material
        }));

        BoundsRect = (RectangleF)Body.Colliders[0].GetAabb(Vector2.Zero);
        BoundsRect = BoundsRect.Inflate(1, 1);
    }
    
    protected virtual void Render(IRenderer2 renderer, RgbaColor color)
    {
        var pos = Body.Position * Services.GameTemplate.TileSize;
        pos = pos.TrimVectorToPixels(Services.GameTemplate.TrimToPixels);
        
        renderer.SpriteRenderer.Draw(_spriteSheet.Resource, pos, 
            _sequence.Frames[0].Source, 
            _sequence.Frames[0].Offset + Origin, 1f, color);
    }

    public void Dispose()
    {
        _spriteSheet?.Dispose();
        _spriteSheet = null;
    }
}