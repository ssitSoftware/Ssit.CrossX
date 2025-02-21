using System;
using System.Collections.Generic;
using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.Games.Physics.Collision.Shapes;
using Ssit.CrossX.Games.Physics.Common;
using Ssit.CrossX.Games.Physics.Dynamics;
using Ssit.CrossX.Games.Physics.Dynamics.Contacts;

namespace Ssit.CrossX.Games.Logic.Objects;

public abstract class Detector(GameObjectsServices services, ObjectCreationParameters parameters)
    : SpriteGameObject(services, parameters), ISwitch
{
    private readonly List<Fixture> _containingFixtures = [];
    
    void ISwitch.Toggle()
    {
    }
    
    public bool IsOn { get; private set; }
    public event Action OnChanged;

    protected void InitializePhysics(Shape detector)
    {
        var transform = new Transform();
        
        detector.ComputeAABB(out var aabb, ref transform, 0);
        
        BoundsRect = new RectangleF(aabb.LowerBound, aabb.UpperBound - aabb.LowerBound);
        BoundsRect = BoundsRect.Inflate(1, 1);
        
        Body.CreateFixture(detector);
        Body.BodyType = BodyType.Static;
        Body.IsSensor = true;
        
        Body.OnCollision += BodyOnOnCollision;
        Body.OnSeparation += BodyOnOnSeparation;
        
        AddState("Off", new State());
        AddState("On", new State());
        
        SetState("Off");
    }

    private void BodyOnOnSeparation(Fixture fixtureA, Fixture fixtureB)
    {
        if (fixtureB.Body.Owner is null)
        {
            return;
        }
        
        _containingFixtures.Remove(fixtureB);
    }

    private bool BodyOnOnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
    {
        if (fixtureB.Body.Owner is null)
        {
            return false;
        }
        
        _containingFixtures.Add(fixtureB);
        return true;
    }

    protected override void OnUpdate(float dt)
    {
        base.OnUpdate(dt);
        
        var isOn = _containingFixtures.Count > 0;

        if (isOn != IsOn)
        {
            IsOn = isOn;
            OnChanged?.Invoke();
            SetState(IsOn ? "On" : "Off");
        }
    }
}