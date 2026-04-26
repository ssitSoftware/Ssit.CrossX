using System.Numerics;
using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Stering;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteringCharacters;

public class WallSlideTimeoutBehavior : SteringBehavior<ISteringCharacter>
{
    private float _elapsed;

    protected override void OnEnter(ISteringCharacter obj)
    {
        _elapsed = 0f;
    }

    protected override bool OnFixedUpdate(ISteringCharacter obj, float dt)
    {
        _elapsed += dt;
        if (_elapsed < obj.PhysicsValues.WallSlideTimeout)
            return false;

        obj.Body.Velocity = obj.Body.Velocity with { X = 0 };
        obj.Body.Position += obj.FaceLeft ? new Vector2(0.1f, 0.0f) : new Vector2(-0.1f, 0.0f);
        
        obj.SetSteringState("Fall");
        return true;
    }
}
