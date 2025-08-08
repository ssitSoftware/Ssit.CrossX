using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Ssit.CrossX;
using Ssit.CrossX.Games.Editor;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.Games.Logic.Objects;
using Ssit.CrossX.Games.Physics.Collision.Shapes;
using Ssit.CrossX.Games.Physics.Dynamics;

namespace Nokemono.Core.Game.Objects.Enemies;

public class Slicer: SpriteGameObject, IHittable
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class Parameters
    {
        [EditorLink(typeof(ITarget))] public int Target { get; set; }
    }
    
    public Vector2 Position => Body.Position;
    
    private ITarget _target;
    
    public Slicer(GameObjectsServices services, ObjectCreationParameters<Parameters> parameters) : base(services, parameters)
    {
        parameters.LinkMap.RequestLink<ITarget>(parameters.Parameters.Target, t => _target = t);
        
        BoundsRect = new RectangleF(-2, -2, 4, 4);
        InitializeSprite("assets:/Game/Objects/Slicer");
        
        Body.CreateFixture(new CircleShape(0.4f, 10)
        {
            Position = new Vector2(0, -0.9f)
        });
        
        Body.CreateFixture(new CircleShape(0.4f, 10)
        {
            Position = new Vector2(0, -0.4f)
        });
        
        Body.Mass = 60;
        
        Body.SetTransform(parameters.Position - new Vector2(0, 0.25f), 0);
        
        Body.IsBullet = true;
        Body.FixedRotation = true;
        Body.Friction = 1f;

        Body.CollisionCategories = Category.Cat1;
        
        AddState("Walk", new State());
        SetState("Walk");
    }

    
    public bool Hit(Vector2 dir, float power)
    {
        Services.CommonSoundContainer.Play("SwordFlesh");
        Body.ApplyLinearImpulse(dir * 10 * Body.Mass, Body.Position);
        return true;
    }
}