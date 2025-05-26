using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Ssit.CrossX.Core;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Input;

namespace Nokemono.Core.Game.Objects.PlayerBehaviors;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class TalkBehavior(Player player, IInputMappings inputMappings, IGameInstance gameInstance) : Behavior
{
    private async void ShowDialog()
    {
        player.SetState("WalkTo");
        
        var faceLeft = player.Body.Position.X > player.NpcCharacterInRange.Position.X;
            
        var dist = player.NpcCharacterInRange.TalkingDistance;
        var targetPosX = player.NpcCharacterInRange.Position.X + (faceLeft ? dist : -dist);

        await player.WalkTo(targetPosX);
        
        player.SetState("Talking");
        player.FaceLeft = faceLeft;
        
        await player.NpcCharacterInRange.StartConversation(player.Body.Position.X);
        
        player.SetState("Idle");
    }

    protected override bool OnUpdate(float dt)
    {
        base.OnUpdate(dt);
        
        if (true == player.NpcCharacterInRange?.CanStartConversation &&
            inputMappings[0].GetButton(GameControls.Operate) == ButtonState.JustPressed)
        {
            ShowDialog();
        }

        return false;
    }
}