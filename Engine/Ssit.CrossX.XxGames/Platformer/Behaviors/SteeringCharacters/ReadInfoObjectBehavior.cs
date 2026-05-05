using Ssit.CrossX.XxGames.Logic;
using Ssit.CrossX.XxGames.Logic.Objects;
using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Steering;
using Ssit.CrossX.XxGames.Physics;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteeringCharacters;

public class ReadInfoObjectBehavior : SteeringBehavior<ISteeringCharacter>
{
    protected override bool OnFixedUpdate(ISteeringCharacter obj, float dt)
    {
        var charAabb = obj.Body.Colliders[0].Aabb;
        var colliders = obj.Body.Simulation.GetColliders(charAabb, obj.Body, colliderType: ColliderType.Trigger);

        foreach (var collider in colliders)
        {
            if (!collider.IsActive)
                continue;

            if (collider.AttachedBody?.Owner is IInfoObject infoObject)
            {
                var text = infoObject.ConsumeInformation();
                if (text != null)
                {
                    obj.Body.Simulation.Messanger.PostMessage(new InfoMessage(text));
                    obj.SetSteeringState("Read");
                }
            }
        }

        return false;
    }
}
