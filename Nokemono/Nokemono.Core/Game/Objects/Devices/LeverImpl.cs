using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.Games.Logic.Objects;

namespace Nokemono.Core.Game.Objects.Devices;

public sealed class LeverImpl : Switch
{
    public LeverImpl(GameObjectsServices services, ObjectCreationParameters<Parameters> parameters) : base(services, parameters)
    {
        InitializeSprite("assets:/Game/Objects/Lever");
        InitializePhysics(parameters, 0.5f);
    }

    public override void Toggle()
    {
        base.Toggle();
        Services.CommonSoundContainer.Play(IsOn ? "RockerSwitchOn" : "RockerSwitchOff");
    }
}