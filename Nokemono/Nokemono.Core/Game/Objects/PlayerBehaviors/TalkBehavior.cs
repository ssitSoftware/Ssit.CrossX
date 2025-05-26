using System.Diagnostics.CodeAnalysis;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Input;

namespace Nokemono.Core.Game.Objects.PlayerBehaviors;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class TalkBehavior(Player player, IInputMappings inputMappings) : Behavior
{
    protected override bool OnUpdate(float dt)
    {
        base.OnUpdate(dt);
        
        if (true == player.NpcCharacterInRange?.CanStartConversation &&
            inputMappings[0].GetButton(GameControls.Operate) == ButtonState.JustPressed)
        {
            player.TalkToNpc(player.NpcCharacterInRange);
        }

        return false;
    }
}