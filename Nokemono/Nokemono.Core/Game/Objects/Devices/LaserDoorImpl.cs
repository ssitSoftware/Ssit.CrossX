using System.Numerics;
using Ssit.CrossX;
using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.Games.Logic.Objects;

namespace Nokemono.Core.Game.Objects.Devices;

public class LaserDoorImpl : MechanicalDoor
{
    public LaserDoorImpl(GameObjectsServices services, ObjectCreationParameters<Parameters> parameters) 
        : base(services, parameters)
    {
        InitializeSprite("assets:/Game/Objects/Laser");
        InitializePhysics(parameters, new Vector2(-0.0625f, 0), new SizeF(0.3125f, 3));
        Body.MaterialIndex = GamePhysics.Materials.Hurt;
    }
}