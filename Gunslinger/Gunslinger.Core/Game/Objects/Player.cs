using System.Numerics;
using Ssit.CrossX;
using Ssit.CrossX.Games.Editor;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.Games.Logic.Objects;
using Ssit.CrossX.Games.Physics.Collision.Shapes;
using Ssit.CrossX.Graphics;

namespace Gunslinger.Core.Game.Objects;

public class Player: SpriteGameObject
{
    public class Parameters
    {
        [EditorFloat(10,20)]
        public float Speed { get; set; }
    }

    public bool FaceLeft
    {
        get => Transform == ImageTransform.FlipHorizontal;
        set => Transform = value ? ImageTransform.FlipHorizontal : ImageTransform.None;
    }

    public Player(GameObjectsServices services, ObjectCreationParameters<Parameters> parameters)
        : base(services, parameters, "assets:/Game/Objects/SwordMaster")
    {
        Sprite.SetSequence("Idle");
        Body.FixedRotation = true;
        
        Body.CreateFixture(new CircleShape(0.3f, 2)
        {
            Position = new Vector2(0,-0.3f)
        });
        
        Body.CreateFixture(new CircleShape(0.3f, 2)
        {
            Position = new Vector2(0, -1.2f)
        });
        
        Body.CreateFixture(new EdgeShape(new Vector2(-0.29f, -0.3f), new Vector2(-0.29f, -1.2f)));
        Body.CreateFixture(new EdgeShape(new Vector2(0.29f, -0.3f), new Vector2(0.29f, -1.2f)));

        BoundsRect = new RectangleF(-1.5f, -4, 3, 5);

        var idleState = new State();
        AddState("Idle", idleState);
    }
}