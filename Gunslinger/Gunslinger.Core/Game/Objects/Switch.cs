using Ssit.CrossX.Games.Editor;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.Games.Logic.Objects;
using Ssit.CrossX.Games.Physics.Collision.Shapes;
using Ssit.CrossX.Games.Physics.Dynamics;
using Ssit.CrossX.Games.Physics.Dynamics.Contacts;

namespace Gunslinger.Core.Game.Objects;

public sealed class Switch : SpriteGameObject, ISwitch, ILogicOperable
{
    public class Parameters
    {
        [Editor] public bool IsOn { get; set; }
    }
    
    public bool IsOn { get; private set; }
    
    public Switch(GameObjectsServices services, ObjectCreationParameters<Parameters> parameters) 
        : base(services, parameters, "assets:/Game/Objects/Switch")
    {
        Body.CreateFixture(new CircleShape(0.5f, 0.1f));
        Body.BodyType = BodyType.Static;
        Body.IsSensor = true;
        
        Body.OnCollision += BodyOnOnCollision;
        Body.OnSeparation += BodyOnOnSeparation;

        AddState("On", new State());
        AddState("Off", new State());
        AddState("TurnOff", new State());
        AddState("TurnOn", new State());
        
        SetState("Off");
    }
    
    private void BodyOnOnSeparation(Fixture fixtureA, Fixture fixtureB)
    {
        if (fixtureB.Body.Owner is ILogicOperator lo)
        {
            lo.SetInRange(this,  fixtureB, false);
        }
    }

    private bool BodyOnOnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
    {
        if (fixtureB.Body.Owner is ILogicOperator lo)
        {
            lo.SetInRange(this,  fixtureB, true);
        }
        return true;
    }

    public void Operate(object by)
    {
        IsOn = !IsOn;
        SetState(IsOn ? "On" : "Off");
    }
}