using System.Diagnostics.CodeAnalysis;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Input;

namespace Nokemono.Core.Game.Objects.PlayerBehaviors;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class OperateBehavior(Player player, IInputMappings inputMappings): Behavior
{
    protected override bool OnUpdate(float dt)
    {
        if (inputMappings[player.PlayerIndex].GetButton(GameControls.Operate) == ButtonState.JustPressed)
        {
            if (player.OperableInRange is not null)
            {
                player.OperableInRange.Operate(player);
                return true;
            }
        }
        
        return false;
    }
}