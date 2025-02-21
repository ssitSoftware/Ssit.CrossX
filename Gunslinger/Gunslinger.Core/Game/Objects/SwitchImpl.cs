using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.Games.Logic.Objects;

namespace Gunslinger.Core.Game.Objects;

public sealed class SwitchImpl : Switch
{
    public SwitchImpl(GameObjectsServices services, ObjectCreationParameters<Parameters> parameters) : base(services, parameters)
    {
        InitializeSprite("assets:/Game/Objects/Switch");
        InitializePhysics(parameters, 0.5f);
    }
}