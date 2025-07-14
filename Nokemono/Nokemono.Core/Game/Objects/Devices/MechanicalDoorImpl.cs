using System.Numerics;
using Ssit.CrossX;
using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.Games.Logic.Objects;

namespace Nokemono.Core.Game.Objects.Devices;

public class MechanicalDoorImpl : MechanicalDoor
{
    public MechanicalDoorImpl(GameObjectsServices services, ObjectCreationParameters<Parameters> parameters) 
        : base(services, parameters)
    {
        InitializeSprite("assets:/Game/Objects/Door");
        InitializePhysics(parameters, Vector2.Zero, new SizeF(0.5f, 3), 0.4f);
    }
}