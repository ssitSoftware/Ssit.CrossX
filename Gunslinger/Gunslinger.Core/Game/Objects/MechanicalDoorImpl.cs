using System.Drawing;
using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.Games.Logic.Objects;

namespace Gunslinger.Core.Game.Objects;

public class MechanicalDoorImpl : MechanicalDoor
{
    public MechanicalDoorImpl(GameObjectsServices services, ObjectCreationParameters<Parameters> parameters) 
        : base(services, parameters)
    {
        InitializeSprite("assets:/Game/Objects/Door");
        InitializePhysics(parameters, new SizeF(0.5f, 3), 0.4f);
    }
}