using System;
using Ssit.CrossX.Audio;
using Ssit.CrossX.Content;
using Ssit.CrossX.Graphics.Sprites;
using Ssit.CrossX.XxGames.Physics;
using Ssit.CrossX.XxGames.Physics.Coliders;
using Ssit.CrossX.XxGames.Platformer.Builders;

namespace Ssit.CrossX.XxGames.Logic.Objects;

public abstract class CollectibleObject : SpriteGameObject2, ICollectible, SpriteInstance.IHandler
{
    private readonly string _idleSequence;

    private ResourceHandle<ISoundEffect> _collectSound;
    
    protected CollectibleObject(string spritePath, string idleSequence, string soundPath, GameObjectsServices services, ObjectCreationParameters parameters)
        : base(services, parameters)
    {
        _idleSequence = idleSequence;

        if (soundPath != null)
        {
            _collectSound = services.ContentManager.Get<ISoundEffect>(soundPath);
        }

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
        Sprite.Handler = this;
        Sprite.SetSequence(_idleSequence);
    }

    bool ICollectible.Collect()
    {
        if (!Body.Colliders[0].IsActive)
            return false;

        Body.Colliders[0].IsActive = false;
        Sprite.SetSequence($"{_idleSequence} Collect");
        
        _collectSound?.Resource.PlayOnce();
        return true;
    }

    public void OnSpriteEvent(SpriteInstance instance, ISpriteEvent @event)
    {
    }

    public void OnSequenceFinished(SpriteInstance instance, string sequenceName, bool reverse)
    {
        if (sequenceName.EndsWith("Collect"))
        {
            Body.Simulation.RemoveBody(Body);
        }
    }

    protected override void OnDispose(bool disposing)
    {
        base.OnDispose(disposing);
        _collectSound?.Dispose();
    }
}
