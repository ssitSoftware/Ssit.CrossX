using System;
using Ssit.CrossX;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.Games.Logic.Objects;
using Ssit.CrossX.Games.Physics.Dynamics;

namespace Nokemono.Core.Game.Objects;

public class PowerPlant : SpriteGameObject, ISwitch
{
    public event Action OnChanged;

    public bool IsOn { get; private set; } = true;
    
    public PowerPlant(GameObjectsServices services, ObjectCreationParameters parameters) : base(services, parameters)
    {
        BoundsRect = new RectangleF(-1, -1, 2, 2);
        InitializeSprite("assets:/Game/Objects/PowerPlant");
        Body.BodyType = BodyType.Static;
        
        AddState("On", new State());
        AddState("TurnOff", new State());
        AddState("Off", new State());
        
        SetState("On");
    }
    
    public void Toggle()
    {
    }
}