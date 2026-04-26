using Ssit.CrossX.Input;
using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Stering;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteringCharacters;

public class JumpBehavior : SteringBehavior<ISteringCharacter>
{
    private bool _jumpRequested;

    protected override bool OnUpdate(ISteringCharacter obj, float dt)
    {
        _jumpRequested = obj.SteringInput.Jump == ButtonState.JustPressed;
        return base.OnUpdate(obj, dt);
    }

    protected override bool OnFixedUpdate(ISteringCharacter obj, float dt)
    {
        if (!_jumpRequested)
            return false;

        if (!obj.SteringParameters.IsOnGround)
        {
            var aabb =  obj.Body.Colliders[0].Aabb;

            if (obj.FaceLeft)
            {
                aabb.Right += 0.3f;
                aabb.Left += 0.1f;
            }
            else
            {
                aabb.Left -= 0.3f;
                aabb.Right -= 0.1f;
            }

            aabb.Top = aabb.Bottom;
            aabb.Bottom += 0.4f;
            
            var colliders = obj.Body.Simulation.GetColliders(aabb, obj.Body);
            if (colliders.Count == 0)
                return false;
        }

        obj.SetSteringState("Raise");
        _jumpRequested = false;
        return true;
    }
}
