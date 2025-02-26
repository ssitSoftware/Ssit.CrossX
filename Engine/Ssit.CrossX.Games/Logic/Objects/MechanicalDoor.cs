using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Ssit.CrossX.Games.Editor;
using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.Games.Physics.Collision.Shapes;
using Ssit.CrossX.Games.Physics.Common;
using Ssit.CrossX.Games.Physics.Dynamics;

namespace Ssit.CrossX.Games.Logic.Objects;

public abstract class MechanicalDoor(GameObjectsServices services, ObjectCreationParameters<MechanicalDoor.Parameters> parameters)
    : SpriteGameObject(services, parameters)
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class Parameters
    {
        [EditorLink(typeof(ISwitch))] public int Switch { get; set; }
        [Editor] public bool Inverse { get; set; }
    }
    
    private ISwitch _switch;
    private bool _inverse;
    private bool _inProgress;
    private bool _isOpen;
    private Fixture _staticFixture;

    protected void InitializePhysics(ObjectCreationParameters<Parameters> parameters, System.Drawing.SizeF size, float topHandleHeight)
    {
        BoundsRect = new RectangleF(-size.Width, -size.Height, size.Width * 2, size.Height * 2);
        BoundsRect = BoundsRect.Inflate(1, 1);
        
        Body.CreateFixture(new EdgeShape(new Vector2(-size.Width / 2, 0), new Vector2(-size.Width / 2, -size.Height)));
        Body.CreateFixture(new EdgeShape(new Vector2(size.Width, 0), new Vector2(size.Width, -size.Height)));
        
        _staticFixture = Body.CreateFixture(new ChainShape(new Vertices([
            new Vector2(-size.Width / 2, -size.Height), 
            new Vector2(size.Width, -size.Height),
            new Vector2(size.Width, -size.Height + topHandleHeight),
            new Vector2(-size.Width / 2, -size.Height + topHandleHeight),
        ]), true));
        
        Body.BodyType = BodyType.Static;
        
        AddState("Opening", null);
        AddState("Open", null);
        AddState("Closed", null);
        AddState("Closing", null);
        
        SetState("Closed");
        
        _inverse = parameters.Parameters.Inverse;
        
        parameters.LinkMap.RequestLink<ISwitch>(parameters.Parameters.Switch, s =>
        {
            _switch = s;
            _isOpen = _switch.IsOn ^ _inverse;
            _inProgress = false;
            UpdateState();
        });
    }

    private void UpdateState()
    {
        _inProgress = false;
        
        SetState(_isOpen ? "Open" : "Closed");
        
        Body.IsSensor = _isOpen;
        _staticFixture.IsSensor = false;
    }
    
    protected override void OnFixedUpdate(float dt)
    {
        base.OnFixedUpdate(dt);

        if (_inProgress)
            return;
        
        var open = (_switch?.IsOn ?? false) ^ _inverse;

        if (open)
        {
            if (!_isOpen)
            {
                _inProgress = true;
                SetState("Opening");
            }
        }
        else
        {
            if (_isOpen)
            {
                _inProgress = true;
                SetState("Closing");
            }
        }
    }

    protected override void OnAnimationFinished(string sequenceName)
    {
        base.OnAnimationFinished(sequenceName);

        switch (sequenceName)
        {
            case "Opening":
                _isOpen = true;
                UpdateState();
                break;
            
            case "Closing":
                _isOpen = false;
                UpdateState();
                break;
        }
    }
}