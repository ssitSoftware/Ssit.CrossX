using Ssit.CrossX.XxGames.Physics;
using Ssit.CrossX.XxGames.Physics.Coliders;
using Ssit.CrossX.XxGames.Platformer.Builders;

namespace Ssit.CrossX.XxGames.Logic.Objects;

public abstract class CollectibleObject : SpriteGameObject2, ICollectible
{
    private readonly string _idleSequence;

    protected CollectibleObject(string spritePath, string idleSequence, GameObjectsServices services, ObjectCreationParameters parameters)
        : base(services, parameters)
    {
        _idleSequence = idleSequence;

        var mainCollider = services.Simulation.CreateCollider(new RectColliderCreationParameters
        {
            Type = ColliderType.Trigger,
            AttachToBody = Body,
            Active = true,
            Size = new SizeF(0.4f, 0.4f),
            Material = Material.Default
        });

        Body.AddColliders(mainCollider);
        Body.IsKinematic = true;

        InitializeSprite(spritePath);
        Sprite.SetSequence(_idleSequence);
    }

    bool ICollectible.Collect()
    {
        if (!Body.Colliders[0].IsActive)
            return false;

        Sprite.SetSequence($"{_idleSequence} Collect");
        Body.Colliders[0].IsActive = true;
        return true;
    }

    protected override void OnSequenceFinished(string sequenceName)
    {
        if (sequenceName.EndsWith("Collect"))
        {
            Body.Simulation.RemoveBody(Body);
            return;
        }
        base.OnSequenceFinished(sequenceName);
    }
}
