using Ssit.CrossX;
using Ssit.CrossX.Content;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.Games.Logic.Narration;
using Ssit.CrossX.Games.Logic.Objects;

namespace Nokemono.Core.Game.Objects;

public class Merchant : NpcCharacter
{
    public Merchant(GameObjectsServices services, IContentManager contentManager, IGameState gameState, INarrationSystem narrationSystem, ObjectCreationParameters parameters)
        : base(services, contentManager, gameState, narrationSystem, parameters)
    {
        NarrationId = "Merchant";
        InitializeSprite("assets:/Game/Objects/Merchant");
        Sprite.SetSequence("Idle");
        
        BoundsRect = new RectangleF(-3, -3, 6, 6);
    }

    protected override void SetSequence(string state)
    {
        state = "Idle";
        base.SetSequence(state);
    }
}