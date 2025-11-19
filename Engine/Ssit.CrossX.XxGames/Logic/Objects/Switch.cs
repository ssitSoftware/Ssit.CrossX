using System;
using System.Diagnostics.CodeAnalysis;
using Ssit.CrossX.XxFormats.Editor;

namespace Ssit.CrossX.XxGames.Logic.Objects;

public abstract class Switch(GameObjectsServices services, ObjectCreationParameters<Switch.Parameters> parameters)
    : SpriteGameObject(services, parameters), ISwitch, ILogicOperable
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class Parameters
    {
        [Editor] public bool IsOn { get; set; }
        [EditorLink(typeof(ISwitch))] public int ToggleAnother { get; set; }
    }
    
    void ILogicOperable.Operate(ILogicOperator by) => Toggle();

    public event Action OnChanged;
    
    public bool IsOn { get; private set; }
    
    private ISwitch _anotherToggle;

    protected void InitializePhysics(ObjectCreationParameters<Parameters> parameters, float detectorRadius)
    {
        BoundsRect = new RectangleF(-detectorRadius * 2, -detectorRadius * 2, detectorRadius * 4, detectorRadius * 4);
        BoundsRect = BoundsRect.Inflate(1, 1);
        
        Body.CreateFixture(new CircleShape(detectorRadius, 0.1f));
        Body.BodyType = BodyType.Static;
        Body.IsSensor = true;
        
        Body.OnCollision += BodyOnOnCollision;
        Body.OnSeparation += BodyOnOnSeparation;

        AddState("On", new State());
        AddState("Off", new State());
        AddState("TurnOff", new State());
        AddState("TurnOn", new State());
        
        SetState("Off");
        
        IsOn = parameters.Parameters.IsOn;

        if (parameters.Parameters.ToggleAnother != 0)
        {
            parameters.LinkMap.RequestLink<ISwitch>(parameters.Parameters.ToggleAnother, UpdateAnotherToggle);
        }
    }

    private void UpdateAnotherToggle(ISwitch another)
    {
        _anotherToggle = another;
        _anotherToggle.OnChanged += UpdateState;
    }

    private void UpdateState()
    {
        var wasOn = CurrentState  is "On" or "TurnOn";
        IsOn = _anotherToggle?.IsOn ?? IsOn;

        if (wasOn != IsOn)
        {
            SetState(IsOn ?  "TurnOn" : "TurnOff");
            OnChanged?.Invoke();
        }
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

    protected override void OnAnimationFinished(string sequenceName)
    {
        base.OnAnimationFinished(sequenceName);
        switch (sequenceName)
        {
            case "TurnOn":
                SetState("On");
                break;
            
            case "TurnOff":
                SetState("Off");
                break;
        }
    }

    public virtual void Toggle()
    {
        if (_anotherToggle != null)
        {
            _anotherToggle.Toggle();
        }
        else
        {
            IsOn = !IsOn;
        }
        UpdateState();
        OnChanged?.Invoke();
    }
}