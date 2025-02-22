using System;
using System.Numerics;
using Ssit.CrossX.Content;
using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.Games.Physics.Collision;
using Ssit.CrossX.Games.Physics.Collision.Shapes;
using Ssit.CrossX.Games.Physics.Dynamics;
using Ssit.CrossX.Games.Rendering;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.Graphics.Sprites;

namespace Ssit.CrossX.Games.Logic.Objects;

public class Pushable(GameObjectsServices services, ObjectCreationParameters parameters): IGameObjectRenderer2, IDisposable, IUpdatable
{
    void IGameObjectRenderer2.Render(IRenderer2 renderer, RgbaColor color) => Render(renderer, color);
    RectangleF IGameObjectRenderer2.Bounds => BoundsRect.Offset(Body.Position);
    int IGameObjectRenderer2.ZOrder { get; } = parameters.ZOrder;

    void IUpdatable.FixedUpdate(float dt)
    {
    }
    
    protected GameObjectsServices Services { get; } = services;
    protected Body Body { get; private set; }
    protected RectangleF BoundsRect { get; set; }
    
    private ResourceHandle<ITexture> _spriteSheet;
    private Sprite.SpriteSequence _sequence;
    
    protected Vector2 Origin { get; private set; }
    
    protected void InitializeSprite(string spritePath)
    {
        using var go = Services.ContentManager.Get<GameObject>(spritePath);
        using var sprite = Services.ContentManager.Get<Sprite>(go.Resource.ResourcePath);

        Origin = go.Resource.Description.Origin;
        _spriteSheet = Services.ContentManager.Get<ITexture>(sprite.Resource.SheetName);
        _sequence = sprite.Resource.GetSequence("Default");
    }
    
    protected void InitializePhysics(Shape shape)
    {
        Body = new Body(Services.World);
        Body.BodyType = BodyType.Dynamic;
        Body.IsBullet = true;
        Body.SetTransform(parameters.Position, 0);
        Body.Owner = this;
        
        Body.CreateFixture(shape);

        Aabb aabb = new Aabb();
        
        switch (shape)
        {
            case ChainShape chain:
                aabb = chain.Vertices.GetAABB();
                break;
            
            case PolygonShape polygon:
                aabb = polygon.Vertices.GetAABB();
                break;
            
            case CircleShape circle:
                aabb = new Aabb(circle.Position - circle.Radius * Vector2.One, circle.Position + circle.Radius * Vector2.One);
                break;
        }
        
        BoundsRect = new RectangleF(aabb.LowerBound, aabb.UpperBound - aabb.LowerBound);
        BoundsRect = BoundsRect.Inflate(1, 1);
    }
    
    protected virtual void Render(IRenderer2 renderer, RgbaColor color)
    {
        const int fullRotationSteps = 24;
        const int quarterRotationSteps = fullRotationSteps / 4;
        const float angle15 = MathF.PI / 12;
        
        var angle = (int)MathF.Round(Body.Rotation / angle15);

        while (angle < 0)
        {
            angle += fullRotationSteps;
        }
        angle %= fullRotationSteps;

        var frame = angle % quarterRotationSteps;
        var transform = (ImageTransform)(angle / quarterRotationSteps);
        
        renderer.SpriteRenderer.Draw(_spriteSheet.Resource, Body.Position * Services.GameTemplate.TileSize, 
            _sequence.Frames[frame].Source, 
            _sequence.Frames[frame].Offset + Origin, 1f, color, transform);
    }

    public void Dispose()
    {
        _spriteSheet?.Dispose();
        _spriteSheet = null;
    }
}