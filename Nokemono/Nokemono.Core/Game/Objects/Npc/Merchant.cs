using System.Numerics;
using Ssit.CrossX;
using Ssit.CrossX.Content;
using Ssit.CrossX.Core;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.Games.Logic.Narration;
using Ssit.CrossX.Games.Logic.Objects;

namespace Nokemono.Core.Game.Objects.Npc;

public class Merchant : NpcCharacter
{
    public Merchant(GameObjectsServices services, IContentManager contentManager, IGameState gameState, INarrationSystem narrationSystem, ICamera camera, IActionScheduler actionScheduler, ObjectCreationParameters parameters)
        : base(services, contentManager, gameState, narrationSystem, camera, actionScheduler, parameters)
    {
        NarrationId = "Merchant";
        InitializeSprite("assets:/Game/Objects/Merchant");
        Sprite.SetSequence("Idle");
        
        BoundsRect = new RectangleF(-3, -3, 6, 6);

        CreateTalkingArea(1.25f, 1.2f);
        CameraOffset = new Vector2(0, -4);
    }

    protected override void SetSequence(string state)
    {
        state = "Idle";
        base.SetSequence(state);
    }
}